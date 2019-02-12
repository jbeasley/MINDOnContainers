using System;
using System.Collections.Generic;
using MINDOnContainers.BuildingBlocks.EventBus.Events;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events
{
    public class DeviceAndPortsAssignedIntegrationEvent : IntegrationEvent
    {
        public int AttachmentId { get; private set; }
        public string DeviceName { get; private set; }
        public int DeviceId { get; private set; }
        public List<PortAssignment> PortAssignments { get; private set; }

        public DeviceAndPortsAssignedIntegrationEvent(int attachmentId, int deviceId, string deviceName, List<PortAssignment> portAssignments)
        {
            AttachmentId = attachmentId;
            DeviceId = deviceId;
            DeviceName = deviceName;
            PortAssignments = portAssignments;
        }

        public class PortAssignment
        {
            public int PortId { get; private set; }
            public string PortName { get; private set; }
            public int PortBandwidthGbps { get; private set; }
        }
    }
}
