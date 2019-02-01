using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class BundleAttachment : Attachment
    {
        // DDD Patterns comment
        // Using private fields, allowed since EF Core 1.1, is a much better encapsulation
        // aligned with DDD Aggregates and Domain Entities (instead of properties and property collections)
        private bool _isBundle;
        private int? _bundleMinLinks;
        private int? _bundleMaxLinks;

        public BundleAttachment(string description, string notes, attachmentBandwidthId, int? tenantId = null, int? routingInstanceId = null, 
        AttachmentRole attachmentRole, int mtuId) : base(description,notes, attachmentBandwidthId, tenantId, routingInstanceId, attachmentRole, mtuId)
        {
            _isBundle = true;       
            _bundleMinLinks = bundleMinLinks ?? bundleMinLinks = this.Interfaces.Count();
        }             
    }
}