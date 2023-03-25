namespace Common.BP.Exceptions.Entity;

public class EntityAlreadyExistingException : Exception
{
    public EntityAlreadyExistingException(string message) : base(message)
    {
        
    }
}