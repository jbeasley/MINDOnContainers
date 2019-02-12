using System;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Application.Commands;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.API.Application.Commands
{
    // Regular CommandHandler
    public class AssignDeviceAndPortsCommandHandler: IRequestHandler<AssignDeviceAndPortsCommand, bool>
    {
        private readonly ISigmaRepository _sigmaRepository;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public AssignDeviceAndPortsCommandHandler(IMediator mediator, ISigmaRepository sigmaRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _sigmaRepository = sigmaRepository ?? throw new ArgumentNullException(nameof(sigmaRepository));
        }

        /// <summary>
        /// Requests the sigma aggregate to assign a device and some port/>
        /// </summary>
        /// <returns>The handle.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<bool> Handle(AssignDeviceAndPortsCommand message, CancellationToken cancellationToken)
        {
            var sigma = await _sigmaRepository.GetAsync();
            sigma.AssignPorts(message.AttachmentId, message.NumPortsRequired, message.PortBandwidthRequired, message.PortPoolId);
           

            return await _sigmaRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
