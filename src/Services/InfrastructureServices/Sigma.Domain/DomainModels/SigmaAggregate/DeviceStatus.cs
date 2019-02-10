using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class DeviceStatus : Enumeration
    {
        public static DeviceStatus Staging = new DeviceStatus(1, nameof(Staging).ToLowerInvariant());
        public static DeviceStatus Production = new DeviceStatus(2, nameof(Production).ToLowerInvariant());
        public static DeviceStatus Retired = new DeviceStatus(3, nameof(Retired).ToLowerInvariant());

        protected DeviceStatus()
        {
        }

        public DeviceStatus(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<DeviceStatus> List() =>
                new[] { Staging, Production, Retired };

        public static DeviceStatus FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new SigmaDomainException($"Possible values for DeviceStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static DeviceStatus From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new SigmaDomainException($"Possible values for DeviceStatus: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
