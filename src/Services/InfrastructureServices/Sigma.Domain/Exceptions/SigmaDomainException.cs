using System;
namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class SigmaDomainException : Exception
    {
        public SigmaDomainException()
        { }

        public SigmaDomainException(string message)
            : base(message)
        { }

        public SigmaDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
