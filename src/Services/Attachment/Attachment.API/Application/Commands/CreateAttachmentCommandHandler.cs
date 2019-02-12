using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using MediatR;
using MINDOnContainers.Services.Attachment.API.Exceptions;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentBandwidthAggregate;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;

namespace MINDOnContainers.Services.Attachment.API.Application.Commands
{
    // Regular CommandHandler
    public class CreateAttachmentCommandHandler: IRequestHandler<CreateAttachmentCommand, AttachmentDTO>
    {
        private readonly IAttachmentRepository _attachmentRepository;
        private readonly IAttachmentRoleRepository _attachmentRoleRepository;
        private readonly IAttachmentBandwidthRepository _attachmentBandwidthRepository;
        private readonly IMediator _mediator;
        private readonly IMapper _mapper;

        // Using DI to inject infrastructure persistence Repositories
        public CreateAttachmentCommandHandler(IMediator mediator, IAttachmentRepository attachmentRepository, 
        IAttachmentRoleRepository attachmentRoleRepository, IAttachmentBandwidthRepository attachmentBandwidthRepository, IMapper mapper)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
            _attachmentRepository = attachmentRepository ?? throw new ArgumentNullException(nameof(attachmentRepository));
            _attachmentRoleRepository = attachmentRoleRepository ?? throw new ArgumentNullException(nameof(attachmentRoleRepository));
            _attachmentBandwidthRepository = attachmentBandwidthRepository ?? throw new ArgumentNullException(nameof(attachmentBandwidthRepository));
            _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        }

        /// <summary>
        /// Request a new attachment.
        /// </summary>
        /// <returns>The handle.</returns>
        /// <param name="message">Message.</param>
        /// <param name="cancellationToken">Cancellation token.</param>
        public async Task<AttachmentDTO> Handle(CreateAttachmentCommand message, CancellationToken cancellationToken)
        {
            var role = await _attachmentRoleRepository.GetAsync(message.AttachmentRoleId) ?? 
                throw new AttachmentApiException($"Could not find the attachment role with ID '{message.AttachmentRoleId}'");

            var bandwidth = await _attachmentBandwidthRepository.GetByValueAsync(message.AttachmentBandwidthGbps) ??
                throw new AttachmentApiException($"Could not find the attachment bandwidth with ID '{message.AttachmentBandwidthGbps}'");

            AttachmentDTO attachmentDto;

            if (message.BundleRequired.GetValueOrDefault())
            {
                var attachment = new BundleAttachment(message.LocationName, message.Description, message.Notes, bandwidth, role, 
                    message.JumboMtuRequired.GetValueOrDefault(), message.PlaneName);

                attachmentDto = _mapper.Map<AttachmentDTO>(attachment);
            }
            else
            {
                var attachment = new SingleAttachment(message.LocationName, message.Description, message.Notes, bandwidth, role,
                    message.JumboMtuRequired.GetValueOrDefault(), message.PlaneName);

                attachmentDto = _mapper.Map<AttachmentDTO>(attachment);
            }

            await _attachmentRepository.UnitOfWork.SaveEntitiesAsync();

            return attachmentDto;
        }
    }

    public class AttachmentDTO
    {
        public int AttachmentId { get; set; }
        public string Name { get; set; }
        public string LocationName { get; set; }
        public string PlaneName { get; set; }
        public int AttachmentBandwidthId { get; set; }
        public string DeviceId { get; set; }
        public string Status { get; set; }
        public int Mtu { get; set; }
        public int? BundleMinLinks { get; set; }
        public int? BundleMaxLinks { get; set; }
        public string RoutingInstanceName { get; set; }
        public int AdministratorSubField { get; set; }
        public int AssignedNumberSubField { get; set; }
    }
}
