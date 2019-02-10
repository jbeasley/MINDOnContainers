using System;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate
{
    public class VifRole : Entity
    {
        public string Name { get; private set; }
        private readonly int? _routingInstanceTypeId;
        public AttachmentRole AttachmentRole { get; private set; }
        public bool IsLayer3Role { get; private set; }       
        public bool IsTenantFacing { get; private set; }
        public bool RequireContractBandwidth { get; private set; }
        public bool RequireRoutingInstance { get; private set; }

        public VifRole(string name, bool isTenantFacing, bool isLayer3Role, bool requireRoutingInstance,
            bool requireContractBandwidth, AttachmentRole attachmentRole, int? routingInstanceTypeId)
        {
            if (string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.IsLayer3Role = IsLayer3Role;
            this.IsTenantFacing = isTenantFacing;
            this.RequireContractBandwidth = requireContractBandwidth;
            this.RequireRoutingInstance = requireRoutingInstance;
            this._routingInstanceTypeId = routingInstanceTypeId;
            this.AttachmentRole = attachmentRole;
        }
    }
}