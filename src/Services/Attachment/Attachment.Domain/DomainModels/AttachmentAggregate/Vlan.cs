using System.Net;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Vlan : Entity
    {
        private Ipv4AddressAndMask _ipv4AddressAndMask;

        public Vlan(Ipv4AddressAndMask ipv4Address = null)
        {
            if (ipv4Address != null) SetIpv4Address(ipv4Address);
        }

        public void SetIpv4Address(Ipv4AddressAndMask ipv4Address)
        {
            if (IPNetwork.TryParse(ipv4Address.Ipv4Address, ipv4Address.Ipv4SubnetMask, out IPNetwork network))
            {
                _ipv4AddressAndMask = new Ipv4AddressAndMask(network.FirstUsable.ToString(), network.Netmask.ToString());
            }
            else
            {
                throw new AttachmentDomainException($"Invalid IPv4 address/mask - '{ipv4Address.Ipv4Address}, {ipv4Address.Ipv4SubnetMask}'");
            }
        }

        public Ipv4AddressAndMask GetIpv4Address() => _ipv4AddressAndMask;
    }
}