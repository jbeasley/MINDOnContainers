using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.Events;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class SingleAttachment : Attachment
    {
        public SingleAttachment(string description, string notes, AttachmentBandwidth attachmentBandwidth,
            AttachmentRole role, Mtu mtu, string locationName, string planeName = null, 
            List<Ipv4AddressAndMask> ipv4Addresses = null, int? tenantId = null) 
            : base(description, notes, attachmentBandwidth, role, mtu, tenantId)
        {
        
            // Create some interfaces and assign IP addresses if the attachment is enabled for layer 3
            base.CreateInterfaces(ipv4Addresses);

            this.AttachmentStatus = AttachmentStatus.AwaitingPortAssignments;

            // Raise a domain event to notify listeners that a new attachment has been initialised and to request some ports to be assigned
            this.AddDomainEvent(new AttachmentInitialisedDomainEvent(this, 1, attachmentBandwidth.BandwidthGbps, locationName, 
                this.AttachmentRole.PortPoolId, planeName));
        }

        public override void AddPorts(List<Port> ports)
        {
            if (!ports.Any()) throw new AttachmentDomainException($"No ports were found when attempting to add ports for attachment '{this.Name}'");
            if (ports.Count > 1) throw new AttachmentDomainException($"Expected 1 port but got {ports.Count} when attempting to add ports " +
                $"for attachment '{this.Name}'.");

            this.Interfaces.Single().AddPort(ports.First());
        }
    }
}