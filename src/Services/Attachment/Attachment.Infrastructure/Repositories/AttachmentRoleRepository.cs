using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Infrastructure;

namespace MINDOnContainers.Services.AttachmentRole.Infrastructure.Repositories
{
    public class AttachmentRoleRepository: IAttachmentRoleRepository
    {
        private readonly AttachmentContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public AttachmentRoleRepository(AttachmentContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public Attachment.Domain.DomainModels.AttachmentRoleAggregate.AttachmentRole Add(Attachment.Domain.DomainModels.AttachmentRoleAggregate.AttachmentRole attachmentRole)
        {
            return _context.AttachmentRoles.Add(attachmentRole).Entity;
        }

        public async Task<Attachment.Domain.DomainModels.AttachmentRoleAggregate.AttachmentRole> GetAsync(int attachmentRoleId)
        {
            var attachmentRole = await _context.AttachmentRoles.FindAsync(attachmentRoleId);
            if (attachmentRole != null)
            {
                await _context.Entry(attachmentRole)
                    .Collection(i => i.VifRoles).LoadAsync();
            }

            return attachmentRole;
        }
    }
}