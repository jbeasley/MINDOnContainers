using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;
namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class Port : Entity
    {
        private readonly int _portId;
        private readonly int _portBandwidthGbps;
        private readonly string _portName;


        public Port(int portId, int portBandwidthGbps, string portName)
        {
            if (portBandwidthGbps <= 0)
            {
                throw new AttachmentDomainException("The port bandwidth must be greater than 0.");
            }

            _portBandwidthGbps = portBandwidthGbps;
            _portId = portId;
            _portName = portName;
        }

        public int GetPortBandwidthGbps() => _portBandwidthGbps;
        public int GetPortId() => _portId;
        public string GetPortName() => _portName;

    }
}