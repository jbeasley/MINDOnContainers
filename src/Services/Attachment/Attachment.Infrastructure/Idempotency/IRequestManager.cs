using System;
using System.Threading.Tasks;

namespace MINDOnContainers.Services.Attachment.Infrastructure.Idempotency
{
    public interface IRequestManager
    {
        Task<bool> ExistAsync(Guid id);

        Task CreateRequestForCommandAsync<T>(Guid id);
    }
}
