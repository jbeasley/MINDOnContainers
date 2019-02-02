using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class AttachmentBandwidth : ValueObject
    {
        public int BandwidthGbps { get; private set; }
        public bool MustBeBundleOrMultiPort { get; private set; }
        public bool SupportedByBundle { get; private set; }
        public bool SupportedByMultiPort { get; private set; }
        public int? BundleOrMultiPortMemberBandwidthGbps { get; private set; }

        public AttachmentBandwidth(int bandwidthGbps, bool mustBeBundleOrMultiPort, 
            bool supportedByBundle, bool supportedByMultiPort, int? bundleOPrMultiPortMemberBandwidthGbps)
        {
            BandwidthGbps = bandwidthGbps;
            MustBeBundleOrMultiPort = mustBeBundleOrMultiPort;
            SupportedByBundle = supportedByBundle;
            SupportedByMultiPort = supportedByMultiPort;

            if (bundleOPrMultiPortMemberBandwidthGbps && !supportedByBundle && !supportedByMultiPort)
            {
                throw new AttachmentDomainException($"Attachment bandwidth of '{bandwidthGbps}' must be supported by a bundle or multiport because the " +
                	$"'{nameof(bundleOPrMultiPortMemberBandwidthGbps}' option is specified with a value of '{bundleOrMultiPortMemberBandwidthGbps}'.");
            }

            BundleOrMultiPortMemberBandwidthGbps = bundleOPrMultiPortMemberBandwidthGbps;
        }
    }
}