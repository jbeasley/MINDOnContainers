using System;
using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate
{
    public class AttachmentRole : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        public bool IsLayer3Role { get; private set; }
        public bool IsTaggedRole { get; private set; }
        public bool RequireContractBandwidth { get; private set; }
        public bool SupportedByBundle { get; private set; }
        public bool SupportedByMultiPort { get; private set; }
        public bool RequireRoutingInstance { get; private set; }
        public int PortPoolId { get; private set; }
        private readonly List<VifRole> _vifRoles;
        public IReadOnlyCollection<VifRole> VifRoles => _vifRoles;

        protected AttachmentRole()
        {
            _vifRoles = new List<VifRole>();
        }

        public AttachmentRole(string name, bool layer3Role, bool taggedRole,
            bool requireContractBandwidth, bool supportedByBundle, bool supportedByMultiPort, bool requireRoutingInstance, 
            int portPoolId) : this()
        { 
            if (!string.IsNullOrEmpty(name)) throw new ArgumentNullException(nameof(name));
            this.Name = name;
            this.IsTaggedRole = taggedRole;
            this.IsLayer3Role = layer3Role;
            this.RequireContractBandwidth = requireContractBandwidth;
            this.SupportedByBundle = supportedByBundle;
            this.SupportedByMultiPort = supportedByMultiPort;
            this.RequireRoutingInstance = requireRoutingInstance;
            this.PortPoolId = portPoolId;
        }
    }
}