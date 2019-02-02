using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class DeviceRole : Entity
    {
        private readonly List<DeviceRoleAttachmentRole> _deviceRoleAttachmentRoles;
        public IReadOnlyCollection<DeviceRoleAttachmentRole> DeviceRoleAttachmentRoles => _deviceRoleAttachmentRoles;
        public bool IsProviderDomainRole { get; private set; }

        public List<int> GetPortPoolIds() => _deviceRoleAttachmentRoles.Select(x => x.GetAttachmentRole().PortPoolId);
    }
}