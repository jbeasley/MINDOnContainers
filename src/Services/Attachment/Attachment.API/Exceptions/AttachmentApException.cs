using System;
namespace MINDOnContainers.Services.Attachment.API.Exceptions
{
    /// <summary>
    /// Exception type for domain exceptions
    /// </summary>
    public class AttachmentApiException : Exception
    {
        public AttachmentApiException()
        { }

        public AttachmentApiException(string message)
            : base(message)
        { }

        public AttachmentApiException(string message, Exception innerException)
            : base(message, innerException)
        { }
    }
}
