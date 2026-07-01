namespace Bombers_System.Domain.Exceptions;

public class ConflictException : Exception 
{
    public ConflictException(string message) : base(message) { }
}