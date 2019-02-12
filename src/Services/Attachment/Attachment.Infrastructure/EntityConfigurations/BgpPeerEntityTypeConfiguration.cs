using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate;

namespace MINDOnContainers.Services.Attachment.Infrastructure.EntityConfigurations
{
    class BgpPeerEntityTypeConfiguration : IEntityTypeConfiguration<BgpPeer>
    {
        public void Configure(EntityTypeBuilder<BgpPeer> bgpPeerConfiguration)
        {
            bgpPeerConfiguration.ToTable("bgpPeer", AttachmentContext.DEFAULT_SCHEMA);
            bgpPeerConfiguration.HasKey(o => o.Id);
            bgpPeerConfiguration.Ignore(b => b.DomainEvents);

            bgpPeerConfiguration.Property(o => o.Id)
                .ForSqlServerUseSequenceHiLo("bgppeerseq", AttachmentContext.DEFAULT_SCHEMA);
                

            bgpPeerConfiguration.Property<string>("Ipv4PeerAddress").IsRequired();
            bgpPeerConfiguration.Property<string>("PeerPasssword").IsRequired();
            bgpPeerConfiguration.Property<int>("Peer2ByteAutonomousSystem").IsRequired();
            bgpPeerConfiguration.Property<int>("MaximumRoutes").IsRequired();
            bgpPeerConfiguration.Property<bool>("IsBfdEnabled").IsRequired();
            bgpPeerConfiguration.Property<bool>("IsMultiHopEnabled").IsRequired();
                
        }
    }
}