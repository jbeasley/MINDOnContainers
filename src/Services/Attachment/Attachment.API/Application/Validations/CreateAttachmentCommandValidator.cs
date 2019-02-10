using FluentValidation;
using MINDOnContainers.Services.Attachment.API.Application.Commands;

namespace MINDOnContainers.Services.Attachment.API.Application.Validations
{
    public class CreateAttachmentCommandValidator : AbstractValidator<CreateAttachmentCommand>
    {
        public CreateAttachmentCommandValidator()
        {
            RuleFor(command => command.AttachmentBandwidthGbps).NotEmpty();
            RuleFor(command => command.LocationName).NotEmpty();
            RuleFor(command => command.AttachmentRoleId).NotEmpty();
            RuleFor(command => command.Description).NotEmpty();
        }
    }
}