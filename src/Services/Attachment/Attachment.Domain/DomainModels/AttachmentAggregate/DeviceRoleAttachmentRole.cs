using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class DeviceRoleAttachmentRole : ValueObject
    {
        private readonly DeviceRole _deviceRole;
        private readonly AttachmentRole _attachmentRole;

        public DeviceRole GetDeviceRole() => _deviceRole;
        public AttachmentRole GetAttachmentRole() => _attachmentRole;
    }
}