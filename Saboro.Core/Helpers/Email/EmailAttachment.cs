namespace Saboro.Core.Helpers.Email;

public class EmailAttachment
{
    public string Name { get; set; }
    public byte[] Buffer { get; set; }
    public string ContentType { get; set; }
}
