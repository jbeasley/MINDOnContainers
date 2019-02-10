using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;
using static MINDOnContainers.Services.Attachment.API.Application.IntegrationEvents.Events.AttachmentDeviceAndPortsAssignedIntegrationEvent;

namespace MINDOnContainers.Services.Attachment.API.Application.Commands
{
    public class SetDeviceAndAttachmentPortsCommand : IRequest<bool>
    {
        public int AttachmentId { get; private set; }

        public int DeviceId { get; private set; }

        public string DeviceName { get; private set; }

        public List<PortAssignment> PortAssignments { get; private set; }

        public SetDeviceAndAttachmentPortsCommand(int attachmentId, int deviceId, string deviceName, List<PortAssignment> portAssignments)
        {
            AttachmentId = attachmentId;
            DeviceId = deviceId;
            DeviceName = deviceName;
            PortAssignments = portAssignments;
        }
    }
}
