namespace Bombers_System.Domain.Exceptions;

public class BadRequestException : Exception
{
    public BadRequestException(string message) : base(message) { }
}