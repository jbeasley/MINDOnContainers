using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate
{
    public class Region : Entity, IAggregateRoot
    {
        public string Name { get; private set; }
        private readonly List<SubRegion> _subRegions;
        public IReadOnlyCollection<SubRegion> SubRegions => _subRegions;

        public Region(string name)
        {
            Name = name;
        }

        public Location GetLocation(string locationName)
        {
            return this.SubRegions.SelectMany(subRegion => subRegion.Locations).SingleOrDefault(location => location.Name == locationName);
        }
    }
}
