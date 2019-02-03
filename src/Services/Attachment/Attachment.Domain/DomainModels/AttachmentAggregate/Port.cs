using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Port : Entity
    {
        private PortStatus _status;
        private readonly int _portBandwidthGbps;
        private int? _tenantId;
        private readonly int _portPoolId;

        public Port(PortStatus status, int portBandwidthGbps, int portPoolId)
        {
            if (portBandwidthGbps <= 0)
            {
                throw new AttachmentDomainException("The port bandwidth must be greater than 0.");
            }

            _status = status;
            _portBandwidthGbps = portBandwidthGbps;
            _portPoolId = portPoolId;
        }

        public int GetPortBandwidthGbps() => _portBandwidthGbps;
        public PortStatus GetPortStatus() => _status;
        public int GetPortPoolId() => _portPoolId;

        public void Assign(int? tenantId)
        {
            _tenantId = tenantId;
            _status = PortStatus.Assigned;
        }

        public void Release()
        {
            _tenantId = null;
            _status = PortStatus.Free;
        }
    }
}