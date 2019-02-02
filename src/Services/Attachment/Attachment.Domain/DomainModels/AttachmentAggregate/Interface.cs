using System.Collections.Generic;
using System.Net;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Interface : Entity
    {
        private readonly Ipv4AddressAndMask _ipv4AddressAndMask;
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;

        protected Interface()
        {
            _ports = new List<Port>();
        }

        public Interface(Ipv4AddressAndMask ipv4Address, List<Port> ports) : this()
        {
            SetIpv4Address(ipv4Address);
            ports.ForEach(port => 
            {
                if (!_ports.Contains(port)) _ports.Add(port));
            }
        }

        public void SetIpv4Address(Ipv4AddressAndMask ipv4Address)
        {
            if (IPNetwork.TryParse(ipv4Address.Ipv4Address, ipv4Address.Ipv4SubnetMask, out network))
            {
                _ipv4AddressAndMask = new Ipv4AddressAndMask()
                {
                    Ipv4Address = network.FirstUseable(),
                    Ipv4SubnetMask = network.NetMask()
                };
            }
            else
            {
                throw new AttachmentDomainException($"Invalid IPv4 address/mask - '{ipv4Address.Ipv4Address}, {ipv4Address.Ipv4SubnetMask}'");
            }
        }
    }
}