using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class DeviceRole : Entity
    {
        private readonly List<DeviceRoleAttachmentRole> _deviceRoleAttachmentRoles;
        public IReadOnlyCollection<DeviceRoleAttachmentRole> DeviceRoleAttachmentRoles => _deviceRoleAttachmentRoles;
    }
}