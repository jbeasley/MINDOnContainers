using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Mtu : Enumeration
    {
        public static Mtu m1500 = new Mtu(1, nameof(m1500).ToLowerInvariant(), 1500);
        public static Mtu m9000 = new Mtu(2, nameof(m9000).ToLowerInvariant(), 9000);

        protected Mtu()
        {
        }

        public Mtu(int id, string name, int value) : base(id, name, value)
        {
        }

        public static IEnumerable<Mtu> List() =>
            new[] { m1500, m9000 };

        public static Mtu FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for Mtu: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static Mtu From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for Mtu: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}