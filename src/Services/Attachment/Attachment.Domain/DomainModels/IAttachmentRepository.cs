using System.Threading.Tasks;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Attachment Aggregate

    public interface IAttachmentRepository : IRepository<Attachment>
    {
        Attachment Add(Attachment attachment);

        void Update(Attachment attachment);

        Task<Attachment> GetAsync(int attachmentId);
    }
}