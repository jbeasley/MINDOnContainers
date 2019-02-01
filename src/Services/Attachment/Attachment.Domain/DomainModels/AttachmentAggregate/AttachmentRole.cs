using MINDOnContainers.Services.Attachment.Domain.Exceptions;
namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
{
    public class AttachmentRole : Entity
    {
        public string Name { get; private set; }
        public bool IsProviderDomainRole { get; private set; }
        public bool IsTenantDomainRole { get; private set; }
        public bool IsLayer3Role { get; private set; }
        public bool IsTaggedRole { get; private set; }
        public bool IsTenantFacing { get; private set; }

        public AttachmentRole(string name, bool isProviderDomainRole, bool isTenantDomainRole, bool layer3Role, bool taggedRole)
        {
            !string.IsNullOrEmpty(name) ? Name = name : throw new NullValueException("A name for the attachment role is required.");
            if (isProviderDomainRole = isTenantDomainRole) throw new AttachmentDomainException("Either the attachment role must be for a provider domain role " +
            	"or for a tenant domain role.");
            IsProviderDomainRole = isProviderDomainRole;
            IsTenantDomainRole = isTenantDomainRole;
            IsTaggedRole = taggedRole;
            IsLayer3Role = layer3Role;
        }
    }
}