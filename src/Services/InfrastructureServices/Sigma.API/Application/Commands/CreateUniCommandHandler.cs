using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.API.Exceptions;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.Commands
{
    // Regular CommandHandler
    public class CreateUniCommandHandler: IRequestHandler<CreateUniCommand, bool>
    {
        private readonly ISigmaRepository _sigmaRepository;
        private readonly IRegionRepository _regionRepository;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public CreateUniCommandHandler(IMediator mediator, ISigmaRepository sigmaRepository, IRegionRepository regionRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _sigmaRepository = sigmaRepository ?? throw new ArgumentNullException(nameof(sigmaRepository));
            _regionRepository = regionRepository ?? throw new ArgumentNullException(nameof(regionRepository));
        }

        /// <summary>
        /// Requests the sigma aggregate to create a new uni.
        /// </summary>
        /// <returns>The handle.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<bool> Handle(CreateUniCommand message, CancellationToken cancellationToken)
        {
            var sigma = await _sigmaRepository.GetAsync() ?? throw new SigmaApiException("'Sigma' does not exist.");
            var location = (from region in await _regionRepository.GetAllAsync()
                            from subRegion in region.SubRegions
                            from loc in subRegion.Locations
                            where loc.Name == message.LocationName
                            select loc)
                            .SingleOrDefault()
                            ?? throw new SigmaApiException($"Location '{message.LocationName}' does not exist.");

            sigma.CreateUni(message.AttachmentId, message.NumPortsRequired, message.PortBandwidthRequiredGbps, message.PortPoolId,
                            location, Plane.FromName(message.PlaneName), message.TenantId); 
                
            return await _sigmaRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
