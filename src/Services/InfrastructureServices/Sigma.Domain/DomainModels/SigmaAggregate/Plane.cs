using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class Plane : Enumeration
    {
        public static Plane Red = new Plane(1, nameof(Red).ToLowerInvariant());
        public static Plane Blue = new Plane(2, nameof(Blue).ToLowerInvariant());

        protected Plane()
        {
        }

        public Plane(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<Plane> List() =>
                new[] { Red, Blue };

        public static Plane FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new SigmaDomainException($"Possible values for Plane: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static Plane From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new SigmaDomainException($"Possible values for Plane: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}
