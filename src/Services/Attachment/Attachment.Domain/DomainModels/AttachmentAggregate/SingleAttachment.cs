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
        public SingleAttachment(string locationName, string description, string notes, AttachmentBandwidth attachmentBandwidth,
            AttachmentRole role, bool enableJumboMtu, string planeName = null, 
            List<Ipv4AddressAndMask> ipv4Addresses = null, int? tenantId = null) 
            : base(locationName, description, notes, attachmentBandwidth, role, enableJumboMtu, planeName, tenantId)
        {
        
            // Create some interfaces and assign IP addresses if the attachment is enabled for layer 3
            base.CreateInterfaces(role, ipv4Addresses);

            this.AttachmentStatus = AttachmentStatus.AwaitingPortAssignments;

            // Raise a domain event to notify listeners that a new attachment has been initialised and to request some ports to be assigned
            this.AddDomainEvent(new AttachmentInitialisedDomainEvent(this, 1, attachmentBandwidth.BandwidthGbps, locationName, 
                role.PortPoolId, planeName));
        }

        public override void AddPorts(AttachmentBandwidth attachmentBandwidth, List<Port> ports)
        {
            // Supplied attachment bandwidth must match that set for the attachment
            if (attachmentBandwidth.Id != this._attachmentBandwidthId)
            {
                throw new AttachmentDomainException($"Attachment bandwidth '{attachmentBandwidth.BandwidthGbps}' is not valid for attachment '{this.Name}' ");
            }

            if (!ports.Any()) throw new AttachmentDomainException($"No ports were found when attempting to add ports for attachment '{this.Name}'");
            if (ports.Count > 1) throw new AttachmentDomainException($"Expected 1 port but got {ports.Count} when attempting to add ports " +
                $"for attachment '{this.Name}'.");

            var port = ports.First();
            if (port.GetPortBandwidthGbps() != attachmentBandwidth.BandwidthGbps)
            {
                throw new AttachmentDomainException("The bandwidth of the assigned port is not compatible with the bandwidth of the attachment.");
            }

            this.Interfaces.Single().AddPort(ports.First());

            this.AttachmentStatus = AttachmentStatus.Active;
        }
    }
}