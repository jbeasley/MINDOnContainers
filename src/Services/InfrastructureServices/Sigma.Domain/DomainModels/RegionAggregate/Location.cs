using System;
using System.Collections.Generic;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate
{
    public class Location : Entity
    {
        private readonly int _subRegionId;
        public string Name { get; private set; }

        public Location(int subregionId, string name)
        {
            this._subRegionId = subregionId;
            this.Name = name;
        }
    }
}
