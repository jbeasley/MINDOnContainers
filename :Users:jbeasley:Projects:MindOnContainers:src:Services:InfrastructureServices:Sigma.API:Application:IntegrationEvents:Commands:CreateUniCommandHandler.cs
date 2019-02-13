using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Application.Commands;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.RegionAggregate;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;
using MINDOnContainers.Services.Sigma.API.Exceptions;

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
        /// Requests the sigma aggregate to assign a device and some port/>
        /// </summary>
        /// <returns>The handle.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<bool> Handle(CreateUniCommand message, CancellationToken cancellationToken)
        {
            var sigma = await _sigmaRepository.GetAsync() ?? throw new SigmaApiException("Could not find 'Sigma'.");
            var location = (from region in await _regionRepository.GetAllAsync()
                            from subRegion in region.SubRegions
                            from loc in subRegion.Locations
                            where loc.Name == message.LocationName
                            select loc)
                            .SingleOrDefault() ?? throw new SigmaApiException($"Could not find location with name '{message.LocationName}'.");

            sigma.CreateUni(message.AttachmentId, message.NumPortsRequired, message.PortBandwidthRequiredGbps, message.PortPoolId, 
                            location, Plane.FromName(message.PlaneName), message.TenantId);
           
            return await _sigmaRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
