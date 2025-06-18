using Microsoft.AspNetCore.Mvc.ViewFeatures;

namespace Saboro.Core.Extensions;

public static class TempDataDictionaryExtensions
{
    private const string ErrorMessageViewDataName = "ErrorMessage";
    private const string SuccessMessageViewDataName = "SuccessMessage";

    public static string ErrorMessage(this ITempDataDictionary tempData)
    {
        return tempData[ErrorMessageViewDataName]?.ToString();
    }

    public static void ErrorMessage(this ITempDataDictionary tempData, object value)
    {
        tempData[ErrorMessageViewDataName] = value.ToString();
    }

    public static string SuccessMessage(this ITempDataDictionary tempData)
    {
        return tempData[SuccessMessageViewDataName]?.ToString();
    }

    public static void SuccessMessage(this ITempDataDictionary tempData, object value)
    {
        tempData[SuccessMessageViewDataName] = value.ToString();
    }
}
