using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class DeviceRoleAttachmentRole : Entity
    {
        public DeviceRole DeviceRole { get; private set; }
        public AttachmentRole AttachmentRole { get; private set; }

    }
}