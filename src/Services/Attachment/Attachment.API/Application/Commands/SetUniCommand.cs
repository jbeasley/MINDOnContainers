using MediatR;
using System.Collections.Generic;

namespace MINDOnContainers.Services.Attachment.API.Application.Commands
{
    public class SetUniCommand : IRequest<bool>
    {
        public int AttachmentId { get; private set; }

        public string UniName { get; private set; }

        public List<string> UniAccessLinkIdentifiers { get; private set; }

        public int? RoutingInstanceId { get; private set; }


        public SetUniCommand(int attachmentId, string uniName, List<string> uniAccessLinkIdentifiers, int? routingInstanceId)
        {
            AttachmentId = attachmentId;
            UniName = uniName;
            UniAccessLinkIdentifiers = uniAccessLinkIdentifiers;
            RoutingInstanceId = routingInstanceId;
        }
    }
}
