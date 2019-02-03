using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Vif : Entity
    {
        public string Name { get; private set; }
        private readonly bool _isLayer3;
        public int VlanTag { get; private set; }
        private readonly int? _tenantId;
        private readonly bool _created;
        private readonly NetworkStatus _networkStatus;
        private RoutingInstance _routingInstance;
        public ContractBandwidthPool ContractBandwidthPool { get; private set; }
        private readonly VifRole _vifRole;
        private readonly Mtu _mtu;
        private readonly List<Vlan> _vlans;
        public IReadOnlyCollection<Vlan> Vlans => _vlans;

        protected Vif()
        {
            _vlans = new List<Vlan>();
            _created = true;
            _networkStatus = NetworkStatus.Init;
        }

        public Vif(bool isLayer3, VifRole role, Mtu mtu, RoutingInstance routingInstance, ContractBandwidthPool contractBandwidthPool, 
            List<Ipv4AddressAndMask> ipv4Addresses, int vlanTag, int? tenantId = null, bool trustReceivedCosAndDscp = false) : this()
        {
            Name = Guid.NewGuid().ToString("N");

            // Must have a tenant specified for a tenant-facing vif
            if (role.IsTenantFacing)
            {
                if (!this._tenantId.HasValue)
                {
                    throw new ArgumentNullException(nameof(tenantId));
                }

                this._tenantId = tenantId;
            }

            _isLayer3 = isLayer3;
            VlanTag = vlanTag;

            _mtu = mtu ?? throw new ArgumentNullException(nameof(mtu));
            _vifRole = role ?? throw new ArgumentNullException(nameof(role));
            ContractBandwidthPool = contractBandwidthPool ?? throw new ArgumentNullException(nameof(contractBandwidthPool));
            this. _routingInstance = routingInstance ?? throw new ArgumentNullException(nameof(routingInstance));
        }

        /// <summary>
        /// Add a vlan.
        /// </summary>
        /// <param name="vlan">Vlan.</param>
        public void AddVlan(Vlan vlan)
        {
            _vlans.Add(vlan);
        }

        /// <summary>
        /// Checks the type of the routing instance to ensure compatiibility with the role of the vif
        /// </summary>
        /// <param name="routingInstanceType">Routing instance type.</param>
        protected void CheckRoutingInstanceType(RoutingInstanceType routingInstanceType)
        {
            if (this._vifRole.IsTenantFacing)
            {
                if (routingInstanceType.Name != RoutingInstanceType.Vrf.Name)
                {
                    throw new AttachmentDomainException($"Routing instance type '{routingInstanceType.Name}' cannot be used to " +
                    	"create a routing instance for a tenant-facing vif.");
                }
            }
        }
    }
}