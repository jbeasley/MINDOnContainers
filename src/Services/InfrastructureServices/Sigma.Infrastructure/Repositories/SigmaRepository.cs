using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.Repositories
{
    public class SigmaRepository: ISigmaRepository
    {
        private readonly SigmaContext _context;

        public IUnitOfWork UnitOfWork
        {
            get
            {
                return _context;
            }
        }

        public SigmaRepository(SigmaContext context)
        {
            _context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<Domain.DomainModels.SigmaAggregate.Sigma> GetAsync()
        {
            var sigma = await _context.Sigma.FindAsync();
            if (sigma != null)
            {
                await _context.Entry(sigma)
                    .Collection(i => i.Devices).LoadAsync();
                await _context.Entry(sigma)
                    .Collection(i => i.Unis).LoadAsync();
            }

            return sigma;
        }

        public void Update(Domain.DomainModels.SigmaAggregate.Sigma sigma)
        {
            _context.Entry(sigma).State = EntityState.Modified;
        }
    }
}