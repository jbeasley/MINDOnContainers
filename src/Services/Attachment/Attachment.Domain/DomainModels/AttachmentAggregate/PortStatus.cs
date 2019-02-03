using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class PortStatus : Enumeration
    {
        public static PortStatus Free = new PortStatus(1, nameof(Free).ToLowerInvariant());
        public static PortStatus Assigned = new PortStatus(2, nameof(Assigned).ToLowerInvariant());
        public static PortStatus Migration = new PortStatus(3, nameof(Migration).ToLowerInvariant());
        public static PortStatus Locked = new PortStatus(4, nameof(Locked).ToLowerInvariant());
        public static PortStatus Reserved = new PortStatus(5, nameof(Reserved).ToLowerInvariant());

        protected PortStatus()
        {
        }

        public PortStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<PortStatus> List() =>
                new[] { Free, Assigned, Migration, Locked, Reserved };

        public static PortStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for PortStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static PortStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for PortStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
