using System.Collections.Generic;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Events
{
    /// <summary>
    /// Event raised when a new UNI is created
    /// </summary>
    public class UniCreatedDomainEvent : INotification
    {
        public int AttachmentId { get; }
        public Uni Uni { get; }

        public UniCreatedDomainEvent(int attachmentId, Uni uni)
        {
            AttachmentId = attachmentId;
            Uni = uni;
        }
    }
}
