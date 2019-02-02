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
        public int PortPoolId { get; private set; }


        public AttachmentRole(string name, bool isTenantFacing, bool layer3Role, bool taggedRole,
            bool requireContractBandwidth, bool supportedByBundle, bool supportedByMultiPort, bool requireRoutingInstance, int portPoolId)
        {
            !string.IsNullOrEmpty(name) ? Name = name : throw new NullArgumentException("A name for the attachment role is required.");
            IsTenantFacing = isTenantFacing;
            IsTaggedRole = taggedRole;
            IsLayer3Role = layer3Role;
            RequireContractBandwidth = requireContractBandwidth;
            SupportedByBundle = supportedByBundle;
            SupportedByMultiPort = supportedByMultiPort;
            RequireRoutingInstance = requireRoutingInstance;
            PortPoolId = portPoolId;
        }
    }
}