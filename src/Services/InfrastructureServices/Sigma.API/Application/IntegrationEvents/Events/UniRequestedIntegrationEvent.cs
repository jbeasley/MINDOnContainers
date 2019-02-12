using System;
using MINDOnContainers.BuildingBlocks.EventBus.Events;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.IntegrationEvents.Events
{
    public class UniRequestedIntegrationEvent : IntegrationEvent
    {
        public int AttachmentId { get; private set; }
        public string LocationName { get; private set; }
        public int NumPortsRequired { get; private set; }
        public int PortBandwidthRequiredGbps { get; private set; }
        public int PortPoolId { get; private set; }
        public string PlaneName { get; private set; }
        public int? TenantId { get; private set; }

        public UniRequestedIntegrationEvent(int attachmentId, string locationName,
            int numPortsRequired, int portBandwidthRequiredGbps, int portPoolId,  string planeName = null, int? tenantId = null)
        {
            AttachmentId = attachmentId;
            LocationName = LocationName;
            PortPoolId = portPoolId;
            PlaneName = planeName;
            PortBandwidthRequiredGbps = portBandwidthRequiredGbps;
            NumPortsRequired = numPortsRequired;
            TenantId = tenantId;
        }
    }
}
