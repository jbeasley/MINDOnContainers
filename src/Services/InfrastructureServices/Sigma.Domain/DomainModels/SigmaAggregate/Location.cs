using System;
using System.Collections.Generic;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class Location : ValueObject
    {
        public string Name { get; private set; }

        public Location(string name)
        {
            Name = name;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Name;
        }
    }
}
