namespace MINDOnContainers.Services.Attachment.API.Application.Queries
{
    using System.Threading.Tasks;

    public interface IAttachmentQueries
    {
        Task<Attachment> GetAttachmentAsync(int id);
    }
}
