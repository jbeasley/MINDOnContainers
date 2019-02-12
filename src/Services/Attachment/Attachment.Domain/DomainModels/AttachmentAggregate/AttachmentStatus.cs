using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class AttachmentStatus : Enumeration
    {
        public static AttachmentStatus CreatedAwaitingUni = new AttachmentStatus(1, nameof(CreatedAwaitingUni).ToLowerInvariant());
        public static AttachmentStatus FailedUni = new AttachmentStatus(2, nameof(FailedUni).ToLowerInvariant());
        public static AttachmentStatus Active = new AttachmentStatus(3, nameof(Active).ToLowerInvariant());

        protected AttachmentStatus()
        {
        }

        public AttachmentStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<AttachmentStatus> List() =>
                new[] { CreatedAwaitingUni, FailedUni, Active };

        public static AttachmentStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for AttachmentStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static AttachmentStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for AttachmentStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
