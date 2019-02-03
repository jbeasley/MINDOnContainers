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

        protected DeviceRole()
        {
            _deviceRoleAttachmentRoles = new List<DeviceRoleAttachmentRole>();
        }

        public List<int> GetPortPoolIds() => _deviceRoleAttachmentRoles
                                                    .Select(x => x.AttachmentRole.PortPoolId)
                                                    .ToList();
    }
}