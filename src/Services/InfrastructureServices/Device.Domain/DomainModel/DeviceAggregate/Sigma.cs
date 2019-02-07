using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.InfrastructureServices.Device.Domain.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Device.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Device.Domain.DomainModels.DeviceAggregate
{
    public class Sigma : Entity, IAggregateRoot
    {
        private readonly List<Device> _devices;
        public IReadOnlyCollection<Device> Devices => _devices;

        protected Sigma()
        {
            _devices = new List<Device>();
        }

        public Sigma(List<Device> devices) : this()
        {
            _devices = devices;
        }

        /// <summary>
        /// Assign ports from the device to the attachment.
        /// </summary>
        /// <returns>The ports.</returns>
        /// <param name="numPortsRequired">Number ports required.</param>
        /// <param name="portBandwidthRequiredGbps">Port bandwidth required gbps.</param>
        protected internal virtual List<Port> AssignPorts(int numPortsRequired, int portBandwidthRequiredGbps, int? tenantId)
        {
            if (numPortsRequired <= 0) throw new DeviceDomainException("Number of ports requested must be greater than 0.");

            // Get free ports from the device which belong to the required port pool and are of the required port bandwidth
            List<Port> ports = this._devices.Select(device => device.Ports.Where(
                                   port =>
                                   port.GetPortStatus() == PortStatus.Free &&
                                   port.GetPortBandwidthGbps() == portBandwidthRequiredGbps &&
                                   port.GetPortPoolId() == this.AttachmentRole.PortPoolId)
                                   .Take(numPortsRequired)
                                   .ToList();

            // Check we have the required number of ports - the 'take' method will only return the number of ports found which may be 
            // less than the required number
            if (ports.Count() != numPortsRequired) throw new DeviceDomainException("Could not find a sufficient number of free ports " +
                $"matching the requirements. {numPortsRequired} ports of {portBandwidthRequiredGbps} Gbps are required but {ports.Count()} free " +
                    "ports were found.");

            // Assign the ports
            ports.ForEach(port => port.Assign(tenantId));

            return ports;
        }
    }
}
