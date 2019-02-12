using System;
using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Events;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate
{
    public class Sigma : Entity, IAggregateRoot
    {
        private readonly List<Device> _devices;
        public IReadOnlyCollection<Device> Devices => _devices;
        private readonly List<int> _locationIds;
        private readonly List<Uni> _unis;
        public IReadOnlyCollection<Uni> Unis => _unis;

        protected Sigma()
        {
            _devices = new List<Device>();
            _locationIds = new List<int>();
        }

        public Sigma(List<Device> devices, List<Plane> planes, List<int> locationIds) : this()
        {
            _devices = devices;
            _locationIds = locationIds;
        }

        /// <summary>
        /// Create a UNI for an attachment.
        /// </summary>
        /// <returns>The ports.</returns>
        /// <param name="numPortsRequired">Number ports required.</param>
        /// <param name="portBandwidthRequiredGbps">Port bandwidth required gbps.</param>
        public Uni CreateUni(int attachmentId, int numPortsRequired, int portBandwidthRequiredGbps, int portPoolId,
        Location location, Plane plane = null, int? tenantId = null)
        {
            if (numPortsRequired <= 0) throw new SigmaDomainException("Number of ports requested must be greater than 0.");

            // Get free ports from the device which belong to the required port pool and are of the required port bandwidth
            var devices = this._devices.Where(device => 
                device.GetLocationId() == location.Id && device.Status == DeviceStatus.Production);

            if (plane != null)
            {
                devices = devices.Where(device => device.Plane == plane);
            }

            if (!devices.Any())
            {
                throw new SigmaDomainException("Could not find any devices in production which satisfy the required location and plane constraints.");
            }

            // Find the device with the most free ports of the required port bandwidth
            var leastLoadedDevice = devices
                            .Aggregate((x, y) =>
                            x.Ports.Count(port => 
                                port.GetPortBandwidthGbps() == portBandwidthRequiredGbps && port.GetPortStatus() == PortStatus.Free) >
                            y.Ports.Count(port => 
                                port.GetPortBandwidthGbps() == portBandwidthRequiredGbps && port.GetPortStatus() == PortStatus.Free) ?
                            x : y);

            List<Port> ports = leastLoadedDevice.Ports.Where(port =>
                                                             port.GetPortStatus() == PortStatus.Free &&
                                                             port.GetPortBandwidthGbps() == portBandwidthRequiredGbps &&
                                                             port.GetPortPoolId() == portPoolId)
                                                      .Take(numPortsRequired)
                                                      .ToList();

            // Check we have the required number of ports - the 'take' method will only return the number of ports found which may be 
            // less than the required number
            if (ports.Count() != numPortsRequired)
            {
                throw new SigmaDomainException("Could not find a sufficient number of free ports " +
                    $"matching the requirements. {numPortsRequired} ports of {portBandwidthRequiredGbps} Gbps are required but {ports.Count()} free " +
                     "ports were found.");
            }

            // Assign the ports
            ports.ForEach(port => port.Assign(attachmentId, tenantId));

            var uni = new Uni(leastLoadedDevice, ports, tenantId: tenantId);
            this._unis.Add(uni);

            this.AddDomainEvent(new UniCreatedDomainEvent(attachmentId, uni));
            return uni;
        }
    }
}
