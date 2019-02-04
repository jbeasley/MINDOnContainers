using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class AttachmentRole : Entity
    {
        public string Name { get; private set; }
        public bool IsLayer3Role { get; private set; }
        public bool IsTaggedRole { get; private set; }
        public bool IsTenantFacing { get; private set; }
        public bool RequireContractBandwidth { get; private set; }
        public bool SupportedByBundle { get; private set; }
        public bool SupportedByMultiPort { get; private set; }
        public bool RequireRoutingInstance { get; private set; }
        private readonly int _routingInstanceTypeId;
        public RoutingInstanceType RoutingInstanceType { get; private set; }
        public int PortPoolId { get; private set; }
        private readonly List<VifRole> _vifRoles;
        public IReadOnlyCollection<VifRole> VifRoles => _vifRoles;

        protected AttachmentRole()
        {
            _vifRoles = new List<VifRole>();
        }

        public AttachmentRole(string name, bool isTenantFacing, bool layer3Role, bool taggedRole,
            bool requireContractBandwidth, bool supportedByBundle, bool supportedByMultiPort, bool requireRoutingInstance, 
            RoutingInstanceType routingInstanceType, int portPoolId) : this()
        { 
            if (!string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            Name = name;
            IsTenantFacing = isTenantFacing;
            IsTaggedRole = taggedRole;
            IsLayer3Role = layer3Role;
            RequireContractBandwidth = requireContractBandwidth;
            SupportedByBundle = supportedByBundle;
            SupportedByMultiPort = supportedByMultiPort;
            RequireRoutingInstance = requireRoutingInstance;
            PortPoolId = portPoolId;
            if (RequireRoutingInstance)
            {
                RoutingInstanceType = routingInstanceType ?? throw new ArgumentNullException(nameof(RoutingInstanceType));
            }
        }
    }
}