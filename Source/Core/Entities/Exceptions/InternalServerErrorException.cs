namespace Entities.Exceptions;

public abstract class InternalServerErrorException : Exception
{
    protected InternalServerErrorException(string message)
        : base(message)
    {

    }
}