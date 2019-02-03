using System;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class VifRole : Entity
    {
        public string Name { get; private set; }
        public RoutingInstanceType RoutingInstanceType { get; private set; }
        public bool IsLayer3Role { get; private set; }       
        public bool IsTenantFacing { get; private set; }
        public bool RequireContractBandwidth { get; private set; }
        public bool RequireRoutingInstance { get; private set; }
       
        public VifRole(string name, bool isTenantFacing, bool isLayer3Role, bool requireRoutingInstance,
            bool requireContractBandwidth, RoutingInstanceType routingInstanceType)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            IsLayer3Role = IsLayer3Role;
            IsTenantFacing = isTenantFacing;
            RequireContractBandwidth = requireContractBandwidth;
            RequireRoutingInstance = requireRoutingInstance;
            if (RequireRoutingInstance) RoutingInstanceType = routingInstanceType ?? throw new ArgumentNullException(nameof(RoutingInstanceType));
        }
    }
}