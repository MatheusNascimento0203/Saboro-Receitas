using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Saboro.Web.Helpers;

public static class PaginationHelper
{
    public static IEnumerable<T> Paginate<T>(this IEnumerable<T> items, int pagina, int tamanhoLote, ViewDataDictionary viewData)
    {
        if (items == null || !items.Any())
        {
            viewData["PaginaAtual"] = 1;
            viewData["TotalPaginas"] = 0;
            viewData["TotalItens"] = 0;
            viewData["TamanhoLote"] = tamanhoLote;
            return Enumerable.Empty<T>();
        }

        int totalItens = items.Count();
        int totalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoLote);
        
        pagina = Math.Max(1, Math.Min(pagina, totalPaginas > 0 ? totalPaginas : 1));
        
        viewData["PaginaAtual"] = pagina;
        viewData["TotalPaginas"] = totalPaginas;
        viewData["TotalItens"] = totalItens;
        viewData["TamanhoLote"] = tamanhoLote;

        return items
            .Skip((pagina - 1) * tamanhoLote)
            .Take(tamanhoLote);
    }
    public static void ConfigurarPaginacao(this Controller controller, int totalItens, int pagina, int tamanhoLote)
    {
        int totalPaginas = (int)Math.Ceiling(totalItens / (double)tamanhoLote);
        pagina = Math.Max(1, Math.Min(pagina, totalPaginas > 0 ? totalPaginas : 1));
        
        controller.ViewBag.PaginaAtual = pagina;
        controller.ViewBag.TotalPaginas = totalPaginas;
        controller.ViewBag.TotalItens = totalItens;
        controller.ViewBag.TamanhoLote = tamanhoLote;
    }
}
