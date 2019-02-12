using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.Commands
{
    public class CreateUniCommand : IRequest<bool>
    {
        [DataMember]
        public int AttachmentId { get; private set; }

        [DataMember]
        public string LocationName { get; private set; }

        [DataMember]
        public string PlaneName { get; private set; }

        [DataMember]
        public int NumPortsRequired { get; private set; }

        [DataMember]
        public int PortBandwidthRequiredGbps { get; private set; }

        [DataMember]
        public int PortPoolId { get; private set; }

        [DataMember]
        public int? TenantId { get; private set; }

        public CreateUniCommand(int attachmentId, string locationName, int portPoolId, int numPortsRequired,
        int portBandwidthRequiredGbps, string planeName = null, int? tenantId = null)
        {
            AttachmentId = attachmentId;
            LocationName = locationName;
            PlaneName = planeName;
            PortPoolId = portPoolId;
            NumPortsRequired = numPortsRequired;
            PortBandwidthRequiredGbps = portBandwidthRequiredGbps;
            TenantId = tenantId;
        }
    }
}
