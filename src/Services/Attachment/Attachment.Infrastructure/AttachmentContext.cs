
using Microsoft.EntityFrameworkCore;

namespace MINDOnContainers.Vif.API.Data
{
    /// <summary>
    /// Creates the microservice database context
    /// </summary>
    public class VifDbContext : DbContext
    {
        public SigmaContext(DbContextOptions<VifDbContext> options) : base(options)
        {
        }

        public DbSet<VifRole> VifRoles { get; set; }
        public DbSet<Vif> Vifs { get; set; }
       
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<VifRole>().ToTable("VifRole");
            modelBuilder.Entity<Vlan>().ToTable("Vlan");
            modelBuilder.Entity<Vif>().ToTable("Vif");
            modelBuilder.Entity<ContractBandwidthPool>().ToTable("ContractBandwidthPool");
            modelBuilder.Entity<ContractBandwidth>().ToTable("ContractBandwidth");


            modelBuilder.Entity<Vif>()
                  .HasOne(c => c.VifRole)
                  .WithMany(e => e.Vifs)
                  .OnDelete(DeleteBehavior.Restrict);

            modelBuilder.Entity<Vif>()
                   .HasOne(c => c.Mtu)
                   .WithMany(e => e.Vifs)
                   .OnDelete(DeleteBehavior.Restrict);

            // Set Indexes to ensure data uniqueness

            modelBuilder.Entity<Vif>()
            .HasIndex(p => new { p.AttachmentID, p.VlanTag }).IsUnique();

            modelBuilder.Entity<Mtu>()
            .HasIndex(p => p.MtuValue).IsUnique();

            modelBuilder.Entity<ContractBandwidth>()
            .HasIndex(p => new { p.BandwidthMbps }).IsUnique();

        }
    }
}