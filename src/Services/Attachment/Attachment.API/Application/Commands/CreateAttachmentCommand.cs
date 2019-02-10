using MediatR;
using System.Collections.Generic;
using System.Runtime.Serialization;

namespace MINDOnContainers.Services.Attachment.API.Application.Commands
{
    public class CreateAttachmentCommand : IRequest<AttachmentDTO>
    {
        [DataMember]
        public string Description { get; private set; }

        [DataMember]
        public string Notes { get; private set; }
        
        [DataMember]
        public string LocationName { get; private set; }

        [DataMember]
        public string PlaneName { get; private set; }

        [DataMember]
        public int AttachmentRoleId { get; private set; }

        [DataMember]
        public int AttachmentBandwidthGbps { get; private set; }

        [DataMember]
        public bool? BundleRequired { get; private set; }

        [DataMember]
        public bool? JumboMtuRequired { get; private set; }

        public CreateAttachmentCommand(string locationName, int attachmentRoleId, int attachmentBandwidthGbps, 
            string description, string notes = null, string planeName = null, bool? bundleRequired = false, bool? jumboMtuRequired = false)
        {
            LocationName = locationName;
            PlaneName = planeName;
            AttachmentRoleId = attachmentRoleId;
            AttachmentBandwidthGbps = attachmentBandwidthGbps;
            BundleRequired = bundleRequired;
            Description = description;
            Notes = notes;
            JumboMtuRequired = jumboMtuRequired;
        }
    }
}
