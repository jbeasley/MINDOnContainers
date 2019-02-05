using System.Collections.Generic;
using MINDOnContainers.Services.Attachment.Domain.Events;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class BundleAttachment : Attachment
    {
        private int? _bundleMinLinks;
        private int? _bundleMaxLinks;

        public BundleAttachment(string description, string notes, AttachmentBandwidth attachmentBandwidth,
            AttachmentRole role, Mtu mtu, Device device, RoutingInstance routingInstance = null, 
            List<Ipv4AddressAndMask> ipv4Addresses = null, int? tenantId = null, int? bundleMinLinks = null, 
            int? bundleMaxLinks = null) 
            : base(description, notes, attachmentBandwidth, role, mtu, device, routingInstance, ipv4Addresses, tenantId)
        { 

            // Check the requested bandwidth for the attachment is supported by a bundle
            if (!attachmentBandwidth.MustBeBundleOrMultiPort && !attachmentBandwidth.SupportedByBundle)
            {
                throw new AttachmentDomainException($"The requested bandwidth, '{attachmentBandwidth.BandwidthGbps} Gbps', " +
                	"is not supported by a bundle attachment.");
            }

            // Assign some ports to the bundle attachment
            var ports = base.AssignPorts(attachmentBandwidth.GetNumberOfPortsRequiredForBundle().Value, attachmentBandwidth.BundleOrMultiPortMemberBandwidthGbps.Value);

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

            // Create some interfaces and assign IP addresses if the attachment is enabled for layer 3
            base.CreateInterfaces(ipv4Addresses, ports);

            // Raise a domain event to notify listeners that a new bundle attachment has been initialised
            this.AddDomainEvent(new AttachmentInitialisedDomainEvent(this));
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
    }
}