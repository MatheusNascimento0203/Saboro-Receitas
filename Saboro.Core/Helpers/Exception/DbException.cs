namespace Saboro.Core.Helpers.Exceptions;

public class DbException(string message) : Exception
{
    public override string Message { get; } = message;
}