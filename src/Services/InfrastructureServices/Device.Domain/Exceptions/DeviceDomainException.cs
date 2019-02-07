using System;
namespace MINDOnContainers.Services.InfrastructureServices.Device.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class DeviceDomainException : Exception
    {
        public DeviceDomainException()
        { }

        public DeviceDomainException(string message)
            : base(message)
        { }

        public DeviceDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
