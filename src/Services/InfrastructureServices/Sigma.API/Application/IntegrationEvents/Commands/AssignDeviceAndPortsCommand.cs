using MediatR;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Application.Commands
{
    public class AssignDeviceAndPortsCommand : IRequest<bool>
    {
        public int AttachmentId { get; private set; }
        public string LocationName { get; private set; }
        public int NumPortsRequired { get; private set; }
        public int PortBandwidthRequired { get; private set; }
        public int PortPoolId { get; private set; }
        public string Plane { get; private set; }

        public AssignDeviceAndPortsCommand(int attachmentId, string locationName,
            int numPortsRequired, int portBandwidthRequiredGbps, int portPoolId, string plane = null)
        {
            AttachmentId = attachmentId;
            LocationName = LocationName;
            PortPoolId = portPoolId;
            Plane = plane;
            PortBandwidthRequired = portBandwidthRequiredGbps;
            NumPortsRequired = numPortsRequired;
        }
    }
}
