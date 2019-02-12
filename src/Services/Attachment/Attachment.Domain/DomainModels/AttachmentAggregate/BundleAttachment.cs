using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentBandwidthAggregate;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;
using MINDOnContainers.Services.Attachment.Domain.Events;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class BundleAttachment : Attachment
    {
        private int _bundleMinLinks;
        private int _bundleMaxLinks;

        public BundleAttachment(int tenantId, string locationName, string description, string notes, AttachmentBandwidth attachmentBandwidth,
            AttachmentRole role, bool enableJumboMtu, string planeName = null,
            List<Ipv4AddressAndMask> ipv4Addresses = null, int? bundleMinLinks = null, 
            int? bundleMaxLinks = null) 
            : base(tenantId, locationName, description, notes, attachmentBandwidth, role, enableJumboMtu, planeName)
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
                this._bundleMinLinks = bundleMinLinks.Value;
            }
            else 
            {
                this._bundleMinLinks = attachmentBandwidth.GetNumberOfPortsRequiredForBundle().Value;
            }

            if (bundleMaxLinks.HasValue)
            {
                this._bundleMaxLinks = bundleMaxLinks.Value;
            }
            else 
            {
                this._bundleMaxLinks = attachmentBandwidth.GetNumberOfPortsRequiredForBundle().Value;
            }

            // Create some interfaces and assign IP addresses if the attachment is enabled for layer 3
            base.CreateInterfaces(role, ipv4Addresses);

            this.AttachmentStatus = AttachmentStatus.CreatedAwaitingUni;

            // Raise a domain event to notify listeners that a new attachment has been created
            this.AddDomainEvent(new AttachmentCreatedDomainEvent(this, attachmentBandwidth.GetNumberOfPortsRequiredForBundle().Value,
            attachmentBandwidth.BundleOrMultiPortMemberBandwidthGbps.Value, locationName, role.PortPoolId, role.RequireRoutingInstance, planeName));
        }             

        /// <summary>
        /// Sets the bundle min and max links parameters.
        /// </summary>
        /// <param name="bundleMinLinks">Bundle minimum links.</param>
        /// <param name="bundleMaxLinks">Bundle max links.</param>
        public void SetBundleLinks(int bundleMinLinks, int bundleMaxLinks)
        {
            var countOfPorts = base.Uni.UniAccessLinkIdentifiers.Count();

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

        public int GetBundleMinLinks() => this._bundleMinLinks;
        public int GetBundleMaxLinks() => this._bundleMaxLinks;
    }
}