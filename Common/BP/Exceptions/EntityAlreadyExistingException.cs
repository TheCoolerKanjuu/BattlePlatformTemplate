namespace Common.BP.Exceptions;

public class EntityAlreadyExistingException : Exception
{
    public EntityAlreadyExistingException(string message) : base(message)
    {
        
    }
}