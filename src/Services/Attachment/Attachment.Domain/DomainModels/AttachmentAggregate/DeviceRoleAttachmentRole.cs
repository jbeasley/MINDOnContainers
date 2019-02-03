using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class DeviceRoleAttachmentRole : ValueObject
    {
        public DeviceRole DeviceRole { get; private set; }
        public AttachmentRole AttachmentRole { get; private set; }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return DeviceRole;
            yield return AttachmentRole;
        }
    }
}