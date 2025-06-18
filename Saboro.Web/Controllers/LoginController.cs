
using Saboro.Core.Interfaces.Helpers;
using Saboro.Web.Extensions;
using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Enums;
using Saboro.Core.Extensions;
using Saboro.Core.Interfaces.Helpers.Email;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Core.Interfaces.Services;
using Saboro.Core.Models;
using Saboro.Web.ViewModels.Login;
using Saboro.Web.ViewModels.Usuario;

namespace Saboro.Web.Controllers;

public class LoginController(INotification notification, IUsuarioRepository usuarioRepository, IUsuarioService usuarioService, IEmailHandler emailHandler, IEncryption encryption) : Controller
{
    private readonly INotification _notification = notification;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly IUsuarioService _usuarioService = usuarioService;
    private readonly IEmailHandler _emailHandler = emailHandler;
    private readonly IEncryption _encryption = encryption;

    [HttpGet, Route("login")]
    public IActionResult Index() => View();

    [HttpPost, Route("login")]
    public async Task<IActionResult> Index(LoginViewModel model)
    {
        if (!model.IsValid(_notification))
        {
            TempData.ErrorMessage(_notification.First());
            return View("Index", model);
        }

        var usuario = await _usuarioRepository.BuscarPorEmailAsync(model.Email);
        if (usuario != null)
        {
            var senhaValida = usuario.Senha.Equals(model.Senha.ToMd5Hash());
            if (senhaValida)
            {
                if (usuario.IdUsuarioStatus != (short)UsuarioStatusEnum.Ativo)
                {
                    TempData.ErrorMessage("Acesso bloqueado, entre em contato com o suporte.");
                    return View("Index", model);
                }

                var nomeUsuario = await _usuarioService.ObterNomePeloIdAsync(usuario.Id);
                var usuarioCookie = new UsuarioCookie
                {
                    Id = usuario.Id,
                    NomeCompleto = nomeUsuario,
                    Email = usuario.Email,
                    ManterConectado = model.ManterConectado
                };

                HttpContext.LogIn(usuarioCookie);

                await _usuarioRepository.AtualizarAsync(usuario.Id, new { TentativasInvalidas = (short)0 });

                TempData.SuccessMessage("Login realizado com sucesso.");
                return RedirectToAction("Index", "Home");
            }

            await _usuarioRepository.AtualizarAsync(usuario.Id, new { TentativasInvalidas = (short)(usuario.TentativasInvalidas + 1) });
            var tentativas = usuario.TentativasInvalidas + 1;

            if (tentativas != (int)TentativasLoginEnum.aviso && tentativas != (int)TentativasLoginEnum.bloqueio)
            {
                TempData.ErrorMessage("Senha e/ou email inválidos!");
                return View("Index", model);
            }

            if ((short)TentativasLoginEnum.aviso == tentativas)
                TempData.ErrorMessage("Ao Completar 4 tentativas, o acesso será bloqueado.");
            else
                TempData.ErrorMessage("Acesso bloqueado, entre em contato com o suporte.");

            if (tentativas == (int)TentativasLoginEnum.bloqueio)
                await _usuarioRepository.AtualizarAsync(usuario.Id, new
                {
                    IdUsuarioStatus = (int)UsuarioStatusEnum.Inativo
                });

            return RedirectToAction(nameof(Index));

        }
        TempData.ErrorMessage("Email ou senha inválidos!");
        return RedirectToAction(nameof(Index));
    }

    [HttpGet("criar-conta")]
    public IActionResult GetCriarConta() => View("NovoUsuario");

    [HttpPost("criar-conta")]
    public async Task<IActionResult> PostCriarConta(UsuarioViewModel model)
    {
        if (!model.IsValid(_notification))
        {
            TempData.ErrorMessage(_notification.First());
        }

        var usuarioExistente = await _usuarioRepository.BuscarPorEmailAsync(model.Email);
        if (usuarioExistente != null)
        {
            TempData.ErrorMessage("Já existe um usuário cadastrado com este e-mail.");
        }

        var usuario = new Usuario
        {
            NomeCompleto = model.NomeCompleto,
            Email = model.Email,
            Senha = model.Senha.ToMd5Hash(),
            IdUsuarioStatus = (short)UsuarioStatusEnum.Ativo
        };

        await _usuarioRepository.AdicionarAsync(usuario);

        var token = _encryption.Encrypt($"{model.Email};{DateTime.Now.AddHours(1).Ticks}");
        var url = Url.Action("AtivarConta", "Login", new { token }, Request.Scheme);

        TempData.SuccessMessage("Conta criada com sucesso!");
        return RedirectToAction(nameof(Index));
    }


    [HttpGet("redefinir-senha/{token}")]
    public IActionResult GetRedefinirSenha(string token)
    {
        var authenticatedUser = HttpContext.GetUser();
        if (authenticatedUser != null)
        {
            TempData.ErrorMessage("Saia do portal primeiro para redefinir senha");
            return RedirectToAction("Index", "Home");
        }
        var model = new RedefinirSenhaViewModel { Token = token };

        return View("RedefinirSenha", model);
    }

    [HttpPost("redefinir-senha")]
    public async Task<IActionResult> RedefinirSenha(RedefinirSenhaViewModel model)
    {
        IActionResult ImpossivelRedefinirSenha()
        {
            return BadRequest("Token inválido!");
        }
        ;

        var token = model.Token.Split(";");
        if (token.Length != 2)
            return ImpossivelRedefinirSenha();

        if (!long.TryParse(token[1], out long ticks))
            return ImpossivelRedefinirSenha();

        DateTime expires;
        try { expires = new DateTime(ticks); } catch { return ImpossivelRedefinirSenha(); }

        if ((expires - DateTime.Now).TotalSeconds < 0)
            return BadRequest("Tempo limite excedido, faça outra solicitação.");

        var email = _encryption.Decrypt(token[0]);
        var usuario = await _usuarioRepository.BuscarPorEmailAsync(email);

        if (usuario == null)
            return ImpossivelRedefinirSenha();

        if (model.Senha != model.SenhaConfirmacao)
            return BadRequest("As senhas não conferem.");

        if (usuario.Senha == model.Senha.ToMd5Hash())
            return BadRequest("A nova senha deve ser diferente de senhas já utilizadas.");

        if (!model.IsValidSenha(model.Senha, _notification))
            return BadRequest(_notification.GetAsString());

        if (usuario.IdUsuarioStatus == (short)UsuarioStatusEnum.Pendente)
            await _usuarioRepository.AtualizarAsync(usuario.Id, new
            {
                Senha = model.Senha.ToMd5Hash(),
                IdUsuarioStatus = (short)UsuarioStatusEnum.Ativo
            });
        else
            await _usuarioRepository.AtualizarAsync(usuario.Id, new
            {
                Senha = model.Senha.ToMd5Hash(),
            });

        TempData.SuccessMessage("Senha alterada com sucesso! Use sua nova senha para fazer login");
        return Ok();
    }

    [HttpGet("ativar-conta")]
    public IActionResult AtivarConta(string token)
    {
        if (string.IsNullOrEmpty(token))
        {
            TempData.ErrorMessage("Token inválido!");
            return RedirectToAction("Index", "Login");
        }

        var model = new AtivarContaViewModel { Token = token };
        return View(model);
    }

    [HttpGet("nao-autorizado")]
    public IActionResult NaoAutorizado()
    {
        return View();
    }


}



