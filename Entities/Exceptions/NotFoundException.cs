namespace Entities.Exceptions
{
    public abstract class NotFoundException : Exception
    {
        //Abstract class used as a base for all individual "not found exception" classes.
        //Inherits from Exception class to represent the errors that happen during application execution.
        protected NotFoundException(string message)
            : base(message)
        {

        }
    }
}
