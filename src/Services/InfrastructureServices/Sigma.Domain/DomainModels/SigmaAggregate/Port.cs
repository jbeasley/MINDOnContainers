using System;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class Port : Entity
    {
        private readonly int _statusId;
        private PortStatus _status;
        private readonly int _portBandwidthGbps;
        private int? _tenantId;
        private int? _attachmentId;
        private readonly int _portPoolId;

        public Port(PortStatus status, int portBandwidthGbps, int portPoolId)
        {
            if (portBandwidthGbps <= 0)
            {
                throw new SigmaDomainException("The port bandwidth must be greater than 0.");
            }

            _status = status ?? throw new ArgumentNullException(nameof(status));
            _statusId = status.Id;
            _portBandwidthGbps = portBandwidthGbps;
            _portPoolId = portPoolId;
        }

        public int GetPortBandwidthGbps() => _portBandwidthGbps;
        public PortStatus GetPortStatus() => _status;
        public int GetPortPoolId() => _portPoolId;

        /// <summary>
        /// Assign a port. The port may also be optionally assigned to a tenant.
        /// </summary>
        /// <param name="tenantId">Tenant identifier.</param>
        public void Assign(int attachmentId, int? tenantId = null)
        {
            _attachmentId = attachmentId;
            _tenantId = tenantId;
            _status = PortStatus.Assigned;
        }

        /// <summary>
        /// Release the port.
        /// </summary>
        public void Release()
        {
            _attachmentId = null;
            _tenantId = null;
            _status = PortStatus.Free;
        }
    }
}
