using System;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    /// <summary>
    /// Entity providing a mapping between Device Roles and Attachment Roles
    /// </summary>
    public class DeviceRoleAttachmentRole : Entity
    {
        private readonly int _deviceRoleId;
        public DeviceRole DeviceRole { get; private set; }
        private readonly int _attachmentRoleId;
        public AttachmentRole AttachmentRole { get; private set; }


        public DeviceRoleAttachmentRole(DeviceRole deviceRole, AttachmentRole attachmentRole)
        {
            DeviceRole = deviceRole ?? throw new ArgumentNullException(nameof(deviceRole));
            _deviceRoleId = deviceRole.Id;
            AttachmentRole = attachmentRole ?? throw new ArgumentNullException(nameof(attachmentRole));
            _attachmentRoleId = attachmentRole.Id;
        }
    }
}