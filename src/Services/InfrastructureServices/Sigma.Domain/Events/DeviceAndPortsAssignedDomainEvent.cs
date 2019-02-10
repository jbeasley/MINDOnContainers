using System.Collections.Generic;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Events
{
    /// <summary>
    /// Event raised when ports are assigned from a device to an attachment.
    /// </summary>
    public class DeviceAndPortsAssignedDomainEvent : INotification
    {
        public int AttachmentId { get; }
        public int DeviceId { get; }
        public List<Port> AssignedPorts { get; }

        public DeviceAndPortsAssignedDomainEvent(int attachmentId, int deviceId, List<Port> assignedPorts)
        {
            AttachmentId = attachmentId;
            DeviceId = deviceId;
            AssignedPorts = assignedPorts;
        }
    }
}
