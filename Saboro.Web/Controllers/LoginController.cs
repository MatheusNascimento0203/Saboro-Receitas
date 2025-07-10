
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

public class LoginController(INotification notification, IUsuarioRepository usuarioRepository, IUsuarioService usuarioService, IEmailHandler emailHandler, IEncryption encryption, IReceitaRepository receitaRepository) : Controller
{
    private readonly INotification _notification = notification;
    private readonly IUsuarioRepository _usuarioRepository = usuarioRepository;
    private readonly IUsuarioService _usuarioService = usuarioService;
    private readonly IEmailHandler _emailHandler = emailHandler;
    private readonly IEncryption _encryption = encryption;
    private readonly IReceitaRepository _receitaRepository = receitaRepository;

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
        if (usuario == null)
        {
            TempData.ErrorMessage("Usuário não cadastrado em nosso sistema.");
            return RedirectToAction(nameof(Index));
        }

        var usuarioPossuiReceitas = await _receitaRepository.BuscarReceitaPorUsuarioAsync(usuario.Id);
        var senhaValida = usuario.Senha.Equals(model.Senha.ToMd5Hash());
        if (senhaValida)
        {
            if (usuario.IdUsuarioStatus != (int)UsuarioStatusEnum.Ativo)
            {
                TempData.ErrorMessage("Acesso bloqueado, entre em contato com o suporte.");
                return RedirectToAction(nameof(Index));
            }

            var nomeUsuario = await _usuarioService.ObterNomePeloIdAsync(usuario.Id);

            var usuarioCookie = new UsuarioCookie
            {
                Id = usuario.Id,
                NomeCompleto = nomeUsuario,
                Email = usuario.Email,
                ManterConectado = model.ManterConectado,
                PossuiReceita = usuarioPossuiReceitas != null && usuarioPossuiReceitas.Any()
            };

            HttpContext.LogIn(usuarioCookie);

            await _usuarioRepository.AtualizarAsync(usuario.Id, new { TentativasInvalidas = (short)0 });

            TempData.SuccessMessage("Login realizado com sucesso.");

            if (usuarioPossuiReceitas != null && usuarioPossuiReceitas.Any())
            {
                return RedirectToAction("Index", "Receita");
            }
            else
            {
                return RedirectToAction("Index", "Home");
            }

        }

        // Senha inválida
        var tentativas = (short)(usuario.TentativasInvalidas + 1);

        if (tentativas >= (short)TentativasLoginEnum.bloqueio)
        {
            await _usuarioRepository.AtualizarAsync(usuario.Id, new
            {
                IdUsuarioStatus = (int)UsuarioStatusEnum.Inativo,
                TentativasInvalidas = tentativas
            });

            TempData.ErrorMessage("Acesso bloqueado, redefina sua senha ou entre em contato com o suporte.");
        }
        else
        {
            await _usuarioRepository.AtualizarAsync(usuario.Id, new
            {
                TentativasInvalidas = tentativas
            });

            if (tentativas == (short)TentativasLoginEnum.aviso)
                TempData.ErrorMessage("Senha e/ou email inválidos! Após 3 tentativas inválidas, o acesso será bloqueado.");
            else
                TempData.ErrorMessage("Senha e/ou email inválidos!");
        }

        return RedirectToAction(nameof(Index));
    }

    [HttpGet("criar-conta")]
    public IActionResult GetCriarConta() => View("NovoUsuario");

    [HttpPost("criar-conta")]
    public async Task<IActionResult> PostCriarConta(UsuarioViewModel model)
    {
        if (!model.IsValid(_notification))
            return BadRequest(_notification.GetAsString());

        var usuarioExistente = await _usuarioRepository.BuscarPorEmailAsync(model.Email);
        if (usuarioExistente != null)
            return BadRequest("Já existe um usuário cadastrado com este e-mail.");


        var usuario = new Usuario
        {
            NomeCompleto = model.NomeCompleto,
            Email = model.Email,
            Senha = model.Senha.ToMd5Hash(),
            IdUsuarioStatus = (int)UsuarioStatusEnum.Ativo
        };

        await _usuarioRepository.AdicionarAsync(usuario);

        var token = _encryption.Encrypt($"{model.Email};{DateTime.Now.AddHours(1).Ticks}");
        var url = Url.Action("AtivarConta", "Login", new { token }, Request.Scheme);

        TempData.SuccessMessage("Conta criada com sucesso!");
        return RedirectToAction(nameof(Index));
    }


    [HttpGet("redefinir-senha")]
    public IActionResult GetRedefinirSenha()
    {
        return View("RedefinirSenha");
    }

    [HttpPost("redefinir-senha")]
    public async Task<IActionResult> PostAlterarSenha(RedefinirSenhaViewModel model)
    {
        if (!model.IsValid(_notification))
            return BadRequest(_notification.GetAsString());

        var usuario = await _usuarioRepository.BuscarPorEmailAsync(model.Email);
        if (usuario == null)
            return BadRequest("Não foi encontrado nenhum usuário com este e-mail.");

        if (usuario.Senha == model.Senha.ToMd5Hash())
            return BadRequest("A nova senha deve ser diferente da senha atual.");

        await _usuarioRepository.AtualizarAsync(usuario.Id, new
        {
            Senha = model.Senha.ToMd5Hash(),
            TentativasInvalidas = (short)0,
            UsuarioUltimaAlteracao = usuario.Id,
            DataUltimaAlteracao = DateTime.Now
        });

        TempData.SuccessMessage("Senha alterada com sucesso! Faça login com sua nova senha.");
        return RedirectToAction("Index", "Login");
    }
}




