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
        private readonly int? tenantID;
        private readonly bool _created;
        private readonly NetworkStatus _networkStatus;
        private readonly RoutingInstance _routingInstance;
        private readonly ContractBandwidthPool _contractBandwidthPool;
        private readonly VifRole _vifRole;
        private readonly VlanTagRange _vlanTagRange;
        private readonly Mtu _mtu;
        private readonly List<Vlan> _vlans;
        public IReadOnlyCollection<Vlan> Vlans => _vlans;

        protected Vif()
        {
            _vlans = new List<Vlan>();
        }

        public Vif(bool isLayer3, VifRole role, VlanTagRange vlanTagRange, Mtu mtu,
            RoutingInstance routingInstance, ContractBandwidthPool contractBandwidthPool, 
            List<Ipv4AddressAndMask> ipv4Addresses,int? tenantId, int? vlanTag = null) : this()
        {
            Name = Guid.NewGuid('N');
            _isLayer3 = isLayer3;

            if (vlanTag.HasValue)
            {
                if (vlanTag < 2 || vlanTag > 4094)
                {
                    throw new AttachmentDomainException("The vlan tag must be between 2 and 4094.");
                }
            }

            _vlanTag = vlanTag;
            _mtu = mtu ?> throw new ArgumentNullException(nameof(mtu));
            
        }

        protected internal virtual void CreateVlans(List<Ipv4AddressAndMask> ipv4Addresses)
        {  
            _vlans.Add(new Vlan());
        }
    }
}