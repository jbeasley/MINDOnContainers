using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;
using Microsoft.EntityFrameworkCore.Storage;
using System;
using System.Data;
using System.Threading;
using System.Threading.Tasks;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentRoleAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure
{
    /// <summary>
    /// Creates the database context
    /// </summary>
    public class AttachmentContext : DbContext, IUnitOfWork
    { 
        public const string DEFAULT_SCHEMA = "attachment";

        public DbSet<Domain.DomainModels.AttachmentAggregate.Attachment> Attachments { get; set; }
        public DbSet<SingleAttachment> SingleAttachments { get; set; }
        public DbSet<BundleAttachment> BundleAttachments { get; set; }
        public DbSet<Domain.DomainModels.AttachmentRoleAggregate.AttachmentRole> AttachmentRoles { get; set; }
        public DbSet<Domain.DomainModels.AttachmentBandwidthAggregate.AttachmentBandwidth> AttachmentBandwidths { get; set; }
        public DbSet<VifRole> VifRoles { get; set; }
        public DbSet<Vif> Vifs { get; set; }
        public DbSet<Ipv4AddressAndMask> Ipv4AddressesAndMasks { get; set; }
        public DbSet<BgpPeer> BgpPeers { get; set; }
        public DbSet<ContractBandwidthPool> ContractBandwidthPools { get; set; }
        public DbSet<Interface> Interfaces { get; set; }
        public DbSet<Port> Ports { get; set; }
        public DbSet<RoutingInstance> RoutingInstances { get; set; }
        public DbSet<Vlan> Vlans { get; set; }

        private readonly IMediator _mediator;
        private IDbContextTransaction _currentTransaction;

        private AttachmentContext(DbContextOptions<AttachmentContext> options) : base(options) { }

        public IDbContextTransaction GetCurrentTransaction => _currentTransaction;

        public AttachmentContext(DbContextOptions<AttachmentContext> options, IMediator mediator) : base(options)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));

            System.Diagnostics.Debug.WriteLine("AttachmentContext::ctor ->" + this.GetHashCode());
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new AttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BundleAttachmentEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VifEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new InterfaceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VifRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentRoleEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentBandwidthEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new RoutingInstanceEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new Ipv4AddressAndMaskEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new VlanEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ContractBandwidthPoolEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new PortEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new BgpPeerEntityTypeConfiguration());
            modelBuilder.ApplyConfiguration(new AttachmentStatusEntityTypeConfiguration());
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

    public class OrderingContextDesignFactory : IDesignTimeDbContextFactory<AttachmentContext>
    {
        public AttachmentContext CreateDbContext(string[] args)
        {
            var optionsBuilder = new DbContextOptionsBuilder<AttachmentContext>()
                .UseSqlServer("Server=.;Initial Catalog=MINDOnContainers.Services.AttachmentDb;Integrated Security=true");

            return new AttachmentContext(optionsBuilder.Options, new NoMediator());
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