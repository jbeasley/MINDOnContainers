using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentBandwidthAggregate;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Infrastructure;

namespace MINDOnContainers.Services.AttachmentBandwidth.Infrastructure.Repositories
{
    public class AttachmentBandwidthRepository: IAttachmentBandwidthRepository
    {
        private readonly AttachmentContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public AttachmentBandwidthRepository(AttachmentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Attachment.Domain.DomainModels.AttachmentBandwidthAggregate.AttachmentBandwidth Add(Attachment.Domain.DomainModels.AttachmentBandwidthAggregate.AttachmentBandwidth attachmentBandwidth)
        {
            return _context.AttachmentBandwidths.Add(attachmentBandwidth).Entity;
        }

        public async Task<Attachment.Domain.DomainModels.AttachmentBandwidthAggregate.AttachmentBandwidth> GetAsync(int attachmentBandwidthId)
        {
            var attachmentBandwidth = await _context.AttachmentBandwidths.FindAsync(attachmentBandwidthId);

            return attachmentBandwidth;
        }

        public async Task<Attachment.Domain.DomainModels.AttachmentBandwidthAggregate.AttachmentBandwidth> GetByValueAsync(int bandwidthGbps)
        {
            var attachmentBandwidth = await _context.AttachmentBandwidths.FirstOrDefaultAsync(x => x.BandwidthGbps == bandwidthGbps);

            return attachmentBandwidth;
        }
    }
}