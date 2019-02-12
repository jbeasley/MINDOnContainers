using System;
namespace MINDOnContainers.Services.Attachment.Domain.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class AttachmentDomainException : Exception
    {
        public AttachmentDomainException()
        { }

        public AttachmentDomainException(string message)
            : base(message)
        { }

        public AttachmentDomainException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
