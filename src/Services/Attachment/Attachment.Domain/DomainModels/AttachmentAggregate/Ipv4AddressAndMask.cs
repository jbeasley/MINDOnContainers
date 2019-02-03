using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    /// <summary>
    /// Model for IPv4 address and subnet mask
    /// </summary>
    public class Ipv4AddressAndMask : ValueObject
    {
        /// <summary>
        /// IPv4 address
        /// </summary>
        /// <value>String denoting an IPv4 address</value>
        public string Ipv4Address { get; private set; }

        /// <summary>
        /// IPv4 subnet mask 
        /// </summary>
        /// <value>String denoting an IPv4 subnet mask</value>
        public string Ipv4SubnetMask { get; private set; }

        public Ipv4AddressAndMask(string ipv4Address, string ipv4SubnetMask)
        {
            Ipv4Address = ipv4Address;
            Ipv4SubnetMask = ipv4SubnetMask;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return Ipv4Address;
            yield return Ipv4SubnetMask;
        }
    }
}
