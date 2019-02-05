using System.Collections.Generic;
using System.Net;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Interface : Entity
    {
        private readonly int ipv4AddressAndMaskId;
        public Ipv4AddressAndMask Ipv4AddressAndMask { get; private set; }
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;
        private readonly List<Vlan> _vlans;
        public IReadOnlyCollection<Vlan> Vlans => _vlans;

        protected Interface()
        {
            _ports = new List<Port>();
            _vlans = new List<Vlan>();
        }

        public Interface(List<Port> ports, Ipv4AddressAndMask ipv4Address = null) : this()
        {
            if (ipv4Address != null) SetIpv4Address(ipv4Address);
            _ports.AddRange(ports.Except(_ports));           
        }

        public void SetIpv4Address(Ipv4AddressAndMask ipv4Address)
        {
            if (IPNetwork.TryParse(ipv4Address.Ipv4Address, ipv4Address.Ipv4SubnetMask, out IPNetwork network))
            {
                this.Ipv4AddressAndMask = new Ipv4AddressAndMask(network.FirstUsable.ToString(), network.Netmask.ToString());
            }
            else
            {
                throw new AttachmentDomainException($"Invalid IPv4 address/mask - '{ipv4Address.Ipv4Address}, {ipv4Address.Ipv4SubnetMask}'");
            }
        }

        /// <summary>
        /// Add a vlan.
        /// </summary>
        /// <param name="vlan">Vlan.</param>
        public void AddVlan(Vlan vlan) => _vlans.Add(vlan);
    }
}