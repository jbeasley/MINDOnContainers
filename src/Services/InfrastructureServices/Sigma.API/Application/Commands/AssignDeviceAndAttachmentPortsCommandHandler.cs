using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;
using MINDOnContainers.Services.Sigma.API.Exceptions;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.Commands
{
    // Regular CommandHandler
    public class AssignDeviceAndsigmaPortsCommandHandler: IRequestHandler<AssignDeviceAndAttachmentPortsCommand, bool>
    {
        private readonly ISigmaRepository _sigmaRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public AssignDeviceAndsigmaPortsCommandHandler(IMediator mediator, ISigmaRepository sigmaRepository, IRegionRepository regionRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _sigmaRepository = sigmaRepository ?? throw new ArgumentNullException(nameof(sigmaRepository));
            _regionRepository = regionRepository ?? throw new ArgumentNullException(nameof(regionRepository));

        }

        /// <summary>
        /// Sets the sigma aggregate with the assigned device ID and ports.
        /// </summary>
        /// <returns>The handle.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<bool> Handle(AssignDeviceAndAttachmentPortsCommand message, CancellationToken cancellationToken)
        {
            var sigma = await _sigmaRepository.GetAsync();
            var location = (from region in await _regionRepository.GetAllAsync()
                            from subRegion in region.SubRegions
                            from loc in subRegion.Locations
                            where loc.Name == message.LocationName
                            select loc)
                            .SingleOrDefault()
                            ?? throw new SigmaApiException($"Location '{message.LocationName}' does not exist.");

            var ports = sigma.AssignPorts(message.AttachmentId, message.NumPortsRequired, message.PortBandwidthRequiredGbps, message.PortPoolId,
                        location, Plane.FromName(message.PlaneName), message.TenantId); 
                
            return await _sigmaRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
