namespace user.application.Exceptions
{
    public class ExternalServiceException : Exception
    {
        public ExternalServiceException(string message, Exception? innerException = null)
            : base(message, innerException)
        {
        }
    }
}
