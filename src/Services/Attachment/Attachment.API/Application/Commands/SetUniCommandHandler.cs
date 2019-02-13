using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;
using MediatR;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentBandwidthAggregate;

namespace MINDOnContainers.Services.Attachment.API.Application.Commands
{
    // Regular CommandHandler
    public class SetUniCommandHandler: IRequestHandler<SetUniCommand, bool>
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IAttachmentBandwidthRepository _attachmentBandwidthRepository;
        private readonly IMediator _mediator;

        // Using DI to inject infrastructure persistence Repositories
        public SetUniCommandHandler(IMediator mediator, IAttachmentRepository attachmentRepository, 
        IAttachmentBandwidthRepository attachmentBandwidthRepository)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _attachmentRepository = attachmentRepository ?? throw new ArgumentNullException(nameof(attachmentRepository));
            _attachmentBandwidthRepository = attachmentBandwidthRepository ?? throw new ArgumentNullException(nameof(attachmentBandwidthRepository));
        }

        /// <summary>
        /// Sets the attachment aggregate with the assigned device ID and ports.
        /// </summary>
        /// <returns>The handle.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<bool> Handle(SetUniCommand message, CancellationToken cancellationToken)
        {
            var attachment = await _attachmentRepository.GetAsync(message.AttachmentId);
            var attachmentBandwidth = await _attachmentBandwidthRepository.GetAsync(attachment.GetAttachmentBandwidthId());

            attachment.SetUni(message.UniName, message.UniAccessLinkIdentifiers);

            return await _attachmentRepository.UnitOfWork.SaveEntitiesAsync();
        }
    }
}
