using System.Collections.Generic;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Events
{
    /// <summary>
    /// Event raised when a the sigma domain aggregate failed to create a new UNI
    /// </summary>
    public class UniCreationFailedDomainEvent : INotification
    {
        public int AttachmentId { get; }

        public UniCreationFailedDomainEvent(int attachmentId, Uni uni)
        {
            AttachmentId = attachmentId;
        }
    }
}
