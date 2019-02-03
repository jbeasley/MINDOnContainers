using System.Collections.Generic;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class SingleAttachment : Attachment
    {
        public SingleAttachment(string description, string notes, AttachmentBandwidth attachmentBandwidth, RoutingInstance routingInstance,
        AttachmentRole role, Mtu mtu, Device device, List<Ipv4AddressAndMask> ipv4Addresses, int? tenantId = null) 
            : base(description, notes, attachmentBandwidth, routingInstance, role, mtu, device, ipv4Addresses, tenantId)
        {
            var ports = base.AssignPorts(1, attachmentBandwidth.BandwidthGbps);
            base.CreateInterfaces(ipv4Addresses, ports); 
        }                            
    }
}