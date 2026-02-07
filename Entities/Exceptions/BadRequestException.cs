namespace Entities.Exceptions
{
    public abstract class BadRequestException : Exception
    {
        //Abstract class used as a base for all individual "bad request exception" classes.
        //Inherits from Exception class to represent the errors that happen during application execution.
        protected BadRequestException(string message)
            : base(message) 
        {
        }
    }
}
