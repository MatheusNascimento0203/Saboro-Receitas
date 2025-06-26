using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Helpers;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Web.Extensions;
using Saboro.Web.ViewModels.Receitas;

namespace Saboro.Web.Controllers.Receita;

[Route("/receita")]
public class ReceitaController(INotification notification, ICategoriaFavoritaRepository categoriaFavoritaRepository, IDificuldadeReceitaRepository dificuldadeReceitaRepository, IReceitaRepository receitaRepository) : Controller
{
    private readonly ICategoriaFavoritaRepository _categoriaFavoritaRepository = categoriaFavoritaRepository;
    private readonly IDificuldadeReceitaRepository _dificuldadeReceitaRepository = dificuldadeReceitaRepository;
    private readonly IReceitaRepository _receitaRepository = receitaRepository;
    private readonly INotification _notification = notification;
    [HttpGet("lista-receita")]
    public async Task<IActionResult> Index()
    {
        var receitas = await _receitaRepository.BuscarReceitaAsync();
        var receitasOrdenadas = receitas.OrderBy(r => r.TituloReceita).ToList();
        return View("Index", receitasOrdenadas);
    }

    [HttpGet("buscar-receita")]
    public async Task<IActionResult> BuscarReceita()
    {
        var receitas = await _receitaRepository.BuscarReceitaAsync();
        var receitasOrdenadas = receitas.OrderBy(r => r.TituloReceita).ToList();
        return PartialView("_CardResultadoReceita", receitasOrdenadas);
    }

    [HttpGet, Route("cadastrar-receita")]
    public async Task<IActionResult> GetCadastrarReceita()
    {
        if (HttpContext.IsAjaxRequest())
            return Ok();

        var categoriasFavoritas = await _categoriaFavoritaRepository.BuscarCategoriaAsync();
        var dificuldades = await _dificuldadeReceitaRepository.BuscarDificuldadeAsync();

        if (categoriasFavoritas == null || dificuldades == null)
            return BadRequest("Nenhuma categoria cadastrada.");

        ViewBag.CategoriaFavorita = categoriasFavoritas;
        ViewBag.DificuldadeReceita = dificuldades;

        return View("Cadastrar");
    }

    [HttpPost, Route("cadatrar-receita")]
    public async Task<IActionResult> PostCadastrarReceita(ReceitaCompletaViewModel model)
    {
        if (model == null)
            return BadRequest("Receita inválida.");

        var usuarioLogado = HttpContext.GetUser();
        if (usuarioLogado == null)
            return BadRequest("Usuário não autenticado.");


        if (model.Receita == null || model.Ingredientes == null || model.ModosPreparo == null)
            return BadRequest("Dados da Receita são obrigatórios.");

        if (!model.Receita.IsValid(notification))
            return BadRequest(_notification.GetAsString());

        foreach (var item in model.Ingredientes)
        {
            if (!item.IsValid(notification))
                return BadRequest(notification.GetAsString());
        }

        foreach (var item in model.ModosPreparo)
        {
            if (!item.IsValid(notification))
                return BadRequest(notification.GetAsString());
        }

        model.Receita.IdUsuario = usuarioLogado.Id;
        var receita = model.Receita.ToModel();

        receita.Ingredientes = model.Ingredientes.Select(x => x.ToModel()).ToList();
        receita.ModoPreparoReceitas = model.ModosPreparo.Select(x => x.ToModel()).ToList();

        await _receitaRepository.AdicionarAsync(receita);

        return Ok("Receita cadastrada com sucesso!");
    }

    [HttpPost, Route("deletar/{id}")]
    public async Task<IActionResult> DeleteReceitaAsync(int id)
    {
        var receita = await _receitaRepository.BuscarReceitaPorIdAsync(id);

        if (receita == null)
            return BadRequest("Receita não encontrada.");

        await _receitaRepository.RemoverAsync(id);
        return Ok("Receita removida com sucesso!");
    }
}
