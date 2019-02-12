using System.Threading.Tasks;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentBandwidthAggregate
{
    //This is just the RepositoryContracts or Interface defined at the Domain Layer
    //as requisite for the Attachment Aggregate

    public interface IAttachmentBandwidthRepository : IRepository<AttachmentBandwidth>
    {
        AttachmentBandwidth Add(AttachmentBandwidth attachmentBandwidth);

        Task<AttachmentBandwidth> GetAsync(int attachmentBandwidthId);

        Task<AttachmentBandwidth> GetByValueAsync(int bandwidthGbps);
    }
}