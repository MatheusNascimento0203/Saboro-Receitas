namespace Saboro.Core.Factories;

public class TemplateNameFactory
{
    public static string BuildPath(string fileName) => $"Templates/Emails/{fileName}.html";
}
