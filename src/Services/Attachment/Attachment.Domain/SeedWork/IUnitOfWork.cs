using System;
using System.Threading;
using System.Threading.Tasks;

namespace MINDOnContainers.Services.Attachment.Domain.SeedWork
{
    public interface IUnitOfWork : IDisposable
    {        
        Task<int> SaveChangesAsync(CancellationToken cancellationToken = default(CancellationToken));
        Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken));
    }
}
