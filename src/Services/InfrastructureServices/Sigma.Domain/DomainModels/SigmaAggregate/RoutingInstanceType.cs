﻿using System.Collections.Generic;
using System.Linq;
using System;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class RoutingInstanceType : Enumeration
    {
        public static RoutingInstanceType Default = new RoutingInstanceType(1, nameof(Default).ToLowerInvariant());
        public static RoutingInstanceType Vrf = new RoutingInstanceType(2, nameof(Vrf).ToLowerInvariant());
       
        public RoutingInstanceType(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<RoutingInstanceType> List() =>
                new[] { Default, Vrf };

        public static RoutingInstanceType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new SigmaDomainException($"Possible values for RoutingInstanceType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static RoutingInstanceType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new SigmaDomainException($"Possible values for RoutingInstanceType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}