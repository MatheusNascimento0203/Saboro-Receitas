using X.PagedList.Mvc.Core;

namespace Saboro.Web.Extensions;

public class PagedListRenderOptionsExtensions
{
    public static PagedListRenderOptions LiceuPager()
    {
        return new PagedListRenderOptions
        {
            UlElementClasses = ["uk-flex uk-flex-middle uk-flex-center uk-margin-remove"],
            LiElementClasses = ["uk-flex"],
            PageClasses = ["uk-button uk-button-default uk-background-muted uk-button-small uk-flex uk-flex-center uk-flex-middle pagination-button"],
            ActiveLiElementClass = "active-page",
            ContainerDivClasses = Enumerable.Empty<string>(),
            LinkToPreviousPageFormat = "<span uk-icon='icon: chevron-left; ratio: 0.7'></span>",
            LinkToNextPageFormat = "<span uk-icon='icon: chevron-right; ratio: 0.7'></span>",
        };
    }
}
