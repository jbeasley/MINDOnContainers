using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class RoutingInstanceType : Enumeration
    {
        public static RoutingInstanceType Default = new RoutingInstanceType(1, nameof(Default).ToLowerInvariant());
        public static RoutingInstanceType ProviderDomainTenantFacingLayer3Vrf = new RoutingInstanceType(2, nameof(ProviderDomainTenantFacingLayer3Vrf).ToLowerInvariant());
        public static RoutingInstanceType ProviderDomainInfrastructureVrf = new RoutingInstanceType(3, nameof(ProviderDomainTenantFacingLayer3Vrf).ToLowerInvariant());

        protected RoutingInstanceType()
        {
        }

        public RoutingInstanceType(int id, string name) : base(id, name)
        {
        }

        public static IEnumerable<RoutingInstanceType> List() =>
                new[] { Default, ProviderDomainTenantFacingLayer3Vrf, ProviderDomainInfrastructureVrf };

        public static RoutingInstanceType FromName(string name)
        {
            var state = List()
                .SingleOrDefault(s => String.Equals(s.Name, name, StringComparison.CurrentCultureIgnoreCase));

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for RoutingInstanceType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }

        public static RoutingInstanceType From(int id)
        {
            var state = List().SingleOrDefault(s => s.Id == id);

            if (state == null)
            {
                throw new OrderingDomainException($"Possible values for RoutingInstanceType: {String.Join(",", List().Select(s => s.Name))}");
            }

            return state;
        }
    }
}