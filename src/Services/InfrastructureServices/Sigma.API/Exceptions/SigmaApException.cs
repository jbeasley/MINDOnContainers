using System;
namespace MINDOnContainers.Services.Sigma.API.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class SigmaApiException : Exception
    {
        public SigmaApiException()
        { }

        public SigmaApiException(string message)
            : base(message)
        { }

        public SigmaApiException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
