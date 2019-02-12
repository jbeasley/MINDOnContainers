using System;
using System.Collections.Generic;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate
{
    public class SubRegion : Entity
    {
        private readonly int _regionId;
        public string Name { get; private set; }
        private readonly List<Location> _locations;
        public IReadOnlyCollection<Location> Locations=> _locations;

        protected SubRegion()
        {
            this._locations = new List<Location>();
        }

        public SubRegion(int regionId, string name) : this()
        {
            this._regionId = regionId;
            this.Name = name;
        }
    }
}
