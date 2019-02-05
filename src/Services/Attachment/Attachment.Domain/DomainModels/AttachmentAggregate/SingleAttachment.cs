using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.Events;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class SingleAttachment : Attachment
    {
        public SingleAttachment(string description, string notes, AttachmentBandwidth attachmentBandwidth,
            AttachmentRole role, Mtu mtu, Device device, RoutingInstance routingInstance = null, 
            List<Ipv4AddressAndMask> ipv4Addresses = null, int? tenantId = null) 
            : base(description, notes, attachmentBandwidth, role, mtu, device, routingInstance, ipv4Addresses, tenantId)
        {
            // Assign a port to the attachment. For a single attachment we only need one.
            var ports = base.AssignPorts(1, attachmentBandwidth.BandwidthGbps);

            // Create some interfaces and assign IP addresses if the attachment is enabled for layer 3
            base.CreateInterfaces(ipv4Addresses, ports);

            // Raise a domain event to notify listeners that a new attachment has been initialised
            this.AddDomainEvent(new AttachmentInitialisedDomainEvent(this));
        }                            
    }
}