using System.Collections.Generic;
using System.Net.IPNetwork;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Interface: Entity 
    {
        private readonly Ipv4AddressAndMask _ipv4AddressAndMask;
        private readonly List<Port> _ports;
        public IReadOnlyCollection<Port> Ports => _ports;

        public Interface(string ipv4Address, string ipv4SubnetMask)
        {
            _ipAddress = ipAddress;
            _subnetMask = subnetMask;
            _ports = new List<Port>()
        }

        public void AddPort(int id)
        {
            if (!_ports.Any(port => port.Id == id))
            {
                _ports.Add(port);
            }
        }

        public void SetIpv4Address(string ipv4Address, string ipv4SubnetMask)
        {
            if (IPNetwork.TryParse(ipv4Address, out network)) 
            {
                _ipv4Address = network.IpAddress;
                _ipv4SubnetMask = network.subnetMask;
            }
        }

    }
}