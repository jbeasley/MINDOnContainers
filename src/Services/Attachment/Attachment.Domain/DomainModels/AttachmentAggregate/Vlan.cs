
namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Vlan : Entity
    {
        private string _ipv4Address;
        private string _ipv4SubnetMask;

        public Vlan(string ipv4Address, string ipv4SubnetMask)
        {
            if (IPNetwork.TryParse(ipv4Address, out network))
            {
                _ipv4Address = network.ipAddress;
                _ipv4SubnetMask = network.subnetMask;
            }
        }
    }
}