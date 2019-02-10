using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.Commands
{
    // Regular CommandHandler
    public class AssignDeviceAndsigmaPortsCommandHandler: IRequestHandler<AssignDeviceAndAttachmentPortsCommand, bool>
    {
        private readonly ISigmaRepository _sigmaRepository;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public AssignDeviceAndsigmaPortsCommandHandler(IMediator mediator, ISigmaRepository sigmaRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _sigmaRepository = sigmaRepository ?? throw new ArgumentNullException(nameof(sigmaRepository));
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

            var ports = sigma.AssignPorts(message.AttachmentId, message.NumPortsRequired, message.PortBandwidthRequiredGbps, message.PortPoolId,
                        sigma.GetLocation(message.LocationName), Plane.FromName(message.PlaneName), message.TenantId); 
                
            return await _sigmaRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
