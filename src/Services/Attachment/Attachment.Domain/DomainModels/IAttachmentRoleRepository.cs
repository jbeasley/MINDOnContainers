using System.Threading.Tasks;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Attachment Aggregate

    public interface IAttachmentRoleRepository : IRepository<AttachmentRole>
    {
        AttachmentRole Add(AttachmentRole attachmentRole);

        Task<AttachmentRole> GetAsync(int attachmentRoleId);
    }
}