using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentBandwidthAggregate;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;
using MINDOnContainers.Services.Attachment.Domain.Events;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class SingleAttachment : Attachment
    {
        public SingleAttachment(int tenantId, string locationName, string description, string notes, AttachmentBandwidth attachmentBandwidth,
            AttachmentRole role, bool enableJumboMtu, string planeName = null,
            List<Ipv4AddressAndMask> ipv4Addresses = null)
            : base(tenantId, locationName, description, notes, attachmentBandwidth, role, enableJumboMtu, planeName)
        {

            // Create some interfaces and assign IP addresses if the attachment is enabled for layer 3
            base.CreateInterfaces(role, ipv4Addresses);

            this.AttachmentStatus = AttachmentStatus.CreatedAwaitingUni;

            // Raise a domain event to notify listeners that a new attachment has been created
            this.AddDomainEvent(new AttachmentCreatedDomainEvent(this, 1, attachmentBandwidth.BandwidthGbps, locationName,
                role.PortPoolId, role.RequireRoutingInstance, planeName));
        }
    } 
}