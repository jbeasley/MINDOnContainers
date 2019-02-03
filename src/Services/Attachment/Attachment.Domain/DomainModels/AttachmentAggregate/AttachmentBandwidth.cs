using System.Collections.Generic;
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
            bool supportedByBundle, bool supportedByMultiPort, int? bundleOrMultiPortMemberBandwidthGbps)
        {
            BandwidthGbps = bandwidthGbps;
            MustBeBundleOrMultiPort = mustBeBundleOrMultiPort;
            SupportedByBundle = supportedByBundle;
            SupportedByMultiPort = supportedByMultiPort;

            if (bundleOrMultiPortMemberBandwidthGbps.HasValue && !supportedByBundle && !supportedByMultiPort)
            {
                throw new AttachmentDomainException($"Attachment bandwidth of '{bandwidthGbps}' must be supported by a bundle or multiport because the " +
                	$"'{nameof(bundleOrMultiPortMemberBandwidthGbps)}' option is specified with a value of '{bundleOrMultiPortMemberBandwidthGbps}'.");
            }

            BundleOrMultiPortMemberBandwidthGbps = bundleOrMultiPortMemberBandwidthGbps;
        }

        public int? GetNumberOfPortsRequiredForBundle()
        {
            if (this.MustBeBundleOrMultiPort || this.SupportedByBundle)
            {
                return this.BandwidthGbps / this.BundleOrMultiPortMemberBandwidthGbps;
            }

            return null;
        }

        public int? GetNumberOfPortsRequiredForMultiPort()
        {
            if (this.MustBeBundleOrMultiPort || this.SupportedByMultiPort)
            {
                return this.BandwidthGbps / this.BundleOrMultiPortMemberBandwidthGbps;
            }

            return null;
        }

        protected override IEnumerable<object> GetAtomicValues()
        {
            yield return BandwidthGbps;
            yield return MustBeBundleOrMultiPort;
            yield return SupportedByBundle;
            yield return SupportedByMultiPort;
            yield return BundleOrMultiPortMemberBandwidthGbps;
        }
    }
}