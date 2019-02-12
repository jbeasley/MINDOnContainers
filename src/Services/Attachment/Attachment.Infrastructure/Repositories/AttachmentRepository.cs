using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Infrastructure.Repositories
{
    public class AttachmentRepository: IAttachmentRepository
    {
        private readonly AttachmentContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public AttachmentRepository(AttachmentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Domain.DomainModels.AttachmentAggregate.Attachment Add(Domain.DomainModels.AttachmentAggregate.Attachment attachment)
        {
            return _context.Attachments.Add(attachment).Entity;
        }

        public async Task<Domain.DomainModels.AttachmentAggregate.Attachment> GetAsync(int attachmentId)
        {
            var attachment = await _context.Attachments.FindAsync(attachmentId);
            if (attachment != null)
            {
                await _context.Entry(attachment)
                    .Collection(i => i.Vifs).LoadAsync();
                await _context.Entry(attachment)
                    .Collection(i => i.Interfaces).LoadAsync();
                await _context.Entry(attachment)
                    .Reference(i => i.Uni).LoadAsync();
            }

            return attachment;
        }

        public void Update(Domain.DomainModels.AttachmentAggregate.Attachment attachment)
        {
            _context.Entry(attachment).State = EntityState.Modified;
        }
    }
}