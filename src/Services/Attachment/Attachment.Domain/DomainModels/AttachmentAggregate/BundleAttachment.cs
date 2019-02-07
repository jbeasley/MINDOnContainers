using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.Events;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class BundleAttachment : Attachment
    {
        private int? _bundleMinLinks;
        private int? _bundleMaxLinks;

        public BundleAttachment(string description, string notes, AttachmentBandwidth attachmentBandwidth,
            AttachmentRole role, Mtu mtu, string locationName, string planeName = null,
            List<Ipv4AddressAndMask> ipv4Addresses = null, int? tenantId = null, int? bundleMinLinks = null, 
            int? bundleMaxLinks = null) 
            : base(description, notes, attachmentBandwidth, role, mtu, tenantId)
        { 

            // Check the requested bandwidth for the attachment is supported by a bundle
            if (!attachmentBandwidth.MustBeBundleOrMultiPort && !attachmentBandwidth.SupportedByBundle)
            {
                throw new AttachmentDomainException($"The requested bandwidth, '{attachmentBandwidth.BandwidthGbps} Gbps', " +
                	"is not supported by a bundle attachment.");
            }
                      
            // Default values for min/max bundle links
            if (bundleMinLinks.HasValue) 
            {
                this._bundleMinLinks = bundleMinLinks;
            }
            else 
            {
                this._bundleMinLinks = attachmentBandwidth.GetNumberOfPortsRequiredForBundle();
            }

            if (bundleMaxLinks.HasValue)
            {
                this._bundleMaxLinks = bundleMaxLinks;
            }
            else 
            {
                this._bundleMaxLinks = attachmentBandwidth.GetNumberOfPortsRequiredForBundle();
            }

            this.AttachmentStatus = AttachmentStatus.AwaitingPortAssignments;

            // Raise a domain event to notify listeners that a new bundle attachment has been initialised and is awaiting port assignments
            this.AddDomainEvent(new AttachmentInitialisedDomainEvent(this, attachmentBandwidth.GetNumberOfPortsRequiredForBundle().Value,
            attachmentBandwidth.BundleOrMultiPortMemberBandwidthGbps.Value, locationName, this.AttachmentRole.PortPoolId, planeName));
        }             

        /// <summary>
        /// Sets the bundle min and max links parameters.
        /// </summary>
        /// <param name="bundleMinLinks">Bundle minimum links.</param>
        /// <param name="bundleMaxLinks">Bundle max links.</param>
        public void SetBundleLinks(int bundleMinLinks, int bundleMaxLinks)
        {
            var countOfPorts = base.GetPorts().Count;

            if (bundleMinLinks < countOfPorts || bundleMinLinks > countOfPorts)
            {
                throw new AttachmentDomainException($"The minimum number of links for bundle attachment '{this.Name}' but be between 1 and {countOfPorts}.");
            }

            if (bundleMaxLinks < countOfPorts || bundleMaxLinks > countOfPorts)
            {
                throw new AttachmentDomainException($"The maximum number of links for bundle attachment '{this.Name}' but be between 1 and {countOfPorts}.");
            }

            if (bundleMaxLinks < bundleMinLinks)
            {
                throw new AttachmentDomainException($"The minimum number of links for bundle attachment '{this.Name}' but be less than or equal to " +
                	"the maximum number of links for the bundle.");
            }

            this._bundleMinLinks = bundleMinLinks;
            this._bundleMaxLinks = bundleMaxLinks;
        }

        public override void AddPorts(List<Port> ports)
        {
            if (!ports.Any()) throw new AttachmentDomainException($"No ports were found when attempting to add ports for attachment '{this.Name}'");
            var numPortsExpected = this.AttachmentBandwidth.GetNumberOfPortsRequiredForBundle();

            if (ports.Count > numPortsExpected) throw new AttachmentDomainException($"Expected {numPortsExpected} ports but got {ports.Count} when attempting " +
            	$"to add ports for attachment '{this.Name}'.");

            ports.ForEach(port => this.Interfaces.Single().AddPort(port));
        }
    }
}