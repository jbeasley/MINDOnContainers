using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.DomainModels.SigmaAggregate;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Domain.SeedWork;
using MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure.EntityConfigurations;

namespace MINDOnContainers.Services.InfrastructureServices.Sigma.Infrastructure
{
    /// <summary>
    /// Creates the database context
    /// </summary>
    public class SigmaContext : DbContext, IUnitOfWork
    { 
        public const string DEFAULT_SCHEMA = "sigma";

        public DbSet<Domain.DomainModels.SigmaAggregate.Sigma> Sigma { get; set; }
        public DbSet<Device> Devices { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<RoutingInstance> RoutingInstances { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        private SigmaContext(DbContextOptions<SigmaContext> options) : base(options) { }

        public IDbContextTransaction GetCurrentTransaction => _currentTransaction;

        public SigmaContext(DbContextOptions<SigmaContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            System.Diagnostics.Debug.WriteLine("SigmaContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new SigmaEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new DeviceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PortEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PortStatusEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoutingInstanceEntityTypeConfiguration());
        }

        public async Task<bool> SaveEntitiesAsync(CancellationToken cancellationToken = default(CancellationToken))
        {
            // Dispatch Domain Events collection. 
            // Doing this right BEFORE committing data (EF SaveChanges) into the DB will make a single transaction including  
            // side effects from the domain event handlers which are using the same DbContext with "InstancePerLifetimeScope" or "scoped" lifetime
            await _mediator.DispatchDomainEventsAsync(this);

            // After executing this line all the changes (from the Command Handler and Domain Event Handlers) 
            // performed through the DbContext will be committed
            var result = await base.SaveChangesAsync();

            return true;
        }

        public async Task BeginTransactionAsync()
        {
            _currentTransaction = _currentTransaction ?? await Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);
        }

        public async Task CommitTransactionAsync()
        {
            try
            {
                await SaveChangesAsync();
                _currentTransaction?.Commit();
            }
            catch
            {
                RollbackTransaction();
                throw;
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }

        public void RollbackTransaction()
        {
            try
            {
                _currentTransaction?.Rollback();
            }
            finally
            {
                if (_currentTransaction != null)
                {
                    _currentTransaction.Dispose();
                    _currentTransaction = null;
                }
            }
        }
    }

    public class OrderingContextDesignFactory : IDesignTimeDbContextFactory<SigmaContext>
    {
        public SigmaContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<SigmaContext>()
                .UseSqlServer("Server=.;Initial Catalog=MINDOnContainers.Services.InfrastructureServices.SigmaDb;Integrated Security=true");

            return new SigmaContext(optionsBuilder.Options, new NoMediator());
        }

        class NoMediator : IMediator
        {
            public Task Publish<TNotification>(TNotification notification, CancellationToken cancellationToken = default(CancellationToken)) where TNotification : INotification
            {
                return Task.CompletedTask;
            }

            public Task Publish(object notification, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }

            public Task<TResponse> Send<TResponse>(IRequest<TResponse> request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.FromResult<TResponse>(default(TResponse));
            }

            public Task Send(IRequest request, CancellationToken cancellationToken = default(CancellationToken))
            {
                return Task.CompletedTask;
            }
        }
    }
}