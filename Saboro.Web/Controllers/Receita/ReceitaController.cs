using Microsoft.AspNetCore.Mvc;
using Saboro.Core.Interfaces.Helpers;
using Saboro.Core.Interfaces.Repositories;
using Saboro.Web.Extensions;

namespace Saboro.Web.Controllers.Receita;

public class ReceitaController(INotification notification, ICategoriaFavoritaRepository categoriaFavoritaRepository, IDificuldadeReceitaRepository dificuldadeReceitaRepository) : Controller
{
    private readonly ICategoriaFavoritaRepository _categoriaFavoritaRepository = categoriaFavoritaRepository;
    private readonly IDificuldadeReceitaRepository _dificuldadeReceitaRepository = dificuldadeReceitaRepository;
    private readonly INotification _notification = notification;
    [HttpGet("receita")]
    public IActionResult Index()
    {
        return View("Index");
    }

    [HttpGet("buscar-receita")]
    public IActionResult BuscarReceita()
    {
        return PartialView("_Buscar");
    }

    [HttpGet("cadastrar-receita")]
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
}
