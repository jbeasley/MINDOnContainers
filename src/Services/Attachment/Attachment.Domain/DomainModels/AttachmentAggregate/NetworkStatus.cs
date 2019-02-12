using System.Collections.Generic;
using System;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class NetworkStatus : Enumeration
    {
        public static NetworkStatus Init = new NetworkStatus(1, nameof(Init).ToLowerInvariant());
        public static NetworkStatus NotStaged = new NetworkStatus(2, nameof(NotStaged).ToLowerInvariant());
        public static NetworkStatus Pending = new NetworkStatus(3, nameof(Pending).ToLowerInvariant());
        public static NetworkStatus Staged = new NetworkStatus(4, nameof(Staged).ToLowerInvariant());
        public static NetworkStatus Active = new NetworkStatus(2, nameof(Active).ToLowerInvariant());

        protected NetworkStatus()
        {
        }

        public NetworkStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<NetworkStatus> List() =>
                new[] { NotStaged, Pending, Staged, Active };

        public static NetworkStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for NetworkStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static NetworkStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new AttachmentDomainException($"Possible values for NetworkStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}