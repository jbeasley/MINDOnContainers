using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class BundleAttachment : Attachment
    {
        private bool _isBundle;
        private int? _bundleMinLinks;
        private int? _bundleMaxLinks;

        public BundleAttachment(string description, string notes, AttachmentBandwidth attachmentBandwidth, RoutingInstance routingInstance,
        AttachmentRole role, Mtu mtu, Device device, List<Ipv4AddressAndMask> ipv4Addresses, int? tenantId = null, int? bundleMinLinks = null, 
            int? bundleMaxLinks = null) 
            : base(description, notes, attachmentBandwidth, routingInstance, role, mtu, device, ipv4Addresses, tenantId)
        { 

            if (!attachmentBandwidth.MustBeBundleOrMultiPort && !attachmentBandwidth.SupportedByBundle)
            {
                throw new AttachmentDomainException($"The requested bandwidth, '{attachmentBandwidth.BandwidthGbps} Gbps', is not supported by a bundle attachment.");
            }

            _isBundle = true;

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

            base.CreateInterfaces(ipv4Addresses, ports); 
        }             

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