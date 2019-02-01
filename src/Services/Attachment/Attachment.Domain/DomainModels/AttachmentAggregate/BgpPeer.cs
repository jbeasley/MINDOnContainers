using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class BgpPeer : Entity
    {
        private string _ipv4PeerAddress;
        private int _peer2ByteAutonomousSystem;
        private int? _maximumRoutes;
        private bool _isBfdEnabled;
        private bool _isMultiHop;
        private string _peerPassword;
        public virtual RoutingInstance RoutingInstance { get; set; }
        public virtual ICollection<VpnTenantCommunityIn> VpnTenantCommunitiesIn { get; set; }
        public virtual ICollection<VpnTenantIpNetworkIn> VpnTenantIpNetworksIn { get; set; }
        public virtual ICollection<VpnTenantCommunityOut> VpnTenantCommunitiesOut { get; set; }
        public virtual ICollection<VpnTenantIpNetworkOut> VpnTenantIpNetworksOut { get; set; }

        /// <summary>
        /// Validate the state of the bgp peer
        /// </summary>
        public virtual void Validate()
        {
            if (this.RoutingInstance == null) throw new IllegalStateException("A routing instance for the BGP peer is required.");
 
            if (!IPAddress.TryParse(this.Ipv4PeerAddress, out IPAddress peerIpv4Address))
                throw new IllegalStateException("The peer address is not a valid IPv4 address");

            if (this.Peer2ByteAutonomousSystem < 1 || this.Peer2ByteAutonomousSystem > 65535)
                throw new IllegalStateException($"The 2 byte autonomous system number requested is not valid for BGP peer '{this.Ipv4PeerAddress}'. The number must be between " +
                    "1 and 65535.");

            // For non-multihop peers, the peer IP address must be reachable from at least one vif or attachment which
            // belongs to the routing instance
            if (!this.IsMultiHop)
            {
                var vlan = this.RoutingInstance.Vifs?.SelectMany(
                    x =>
                    x.Vlans)
                     .ToList()
                     .FirstOrDefault(
                        x =>
                        {
                            return IPNetwork.TryParse(x.IpAddress, x.SubnetMask, out IPNetwork network) && network.Contains(peerIpv4Address);
                        });

                var iface = this.RoutingInstance.Attachments?.SelectMany(
                    x =>
                    x.Interfaces)
                     .ToList()
                     .FirstOrDefault(
                        x =>
                        {
                            return IPNetwork.TryParse(x.IpAddress, x.SubnetMask, out IPNetwork network) && network.Contains(peerIpv4Address);
                        });

                if (vlan == null && iface == null)
                    throw new IllegalStateException($"The peer address '{this.Ipv4PeerAddress}' is not contained by any network which is " +
                        $"directly reachable from routing instance '{this.RoutingInstance.Name}'. Check that the IP address for at least one vif or " +
                        $"attachment belonging to the routing instance is in the same IPv4 network as the bgp peer.");
            }
        }

        /// <summary>
        /// Validate a bgp peer can be deleted
        /// </summary>
        public virtual void ValidateDelete()
        {
            var sb = new StringBuilder();
            this.VpnTenantCommunitiesIn
               .ToList()
               .ForEach(x =>
                    {
                        sb.Append($"BGP Peer '{this.Ipv4PeerAddress}' cannot be deleted because community " +
                        $"'{x.TenantCommunity.Name}' is applied to the inbound policy of ");

                        if (x.AttachmentSet != null)
                        {
                            sb.Append($"attachment set '{x.AttachmentSet.Name}'.").Append("\r\n");
                        }
                        else
                        {
                            sb.Append($"tenant domain device '{x.BgpPeer.RoutingInstance.Device.Name}'.").Append("\r\n");
                        }
                   }
            );

            this.VpnTenantCommunitiesOut
                .ToList()
                .ForEach(x =>
                    {
                        sb.Append($"BGP Peer '{this.Ipv4PeerAddress}' cannot be deleted because community " +
                        $"'{x.TenantCommunity.Name}' is applied to the outbound policy of ");

                        if (x.AttachmentSet != null)
                        {
                            sb.Append($"attachment set '{x.AttachmentSet.Name}'.").Append("\r\n");
                        }
                        else
                        {
                            sb.Append($"tenant domain device '{x.BgpPeer.RoutingInstance.Device.Name}'.").Append("\r\n");
                        }
                    }
                );

            this.VpnTenantIpNetworksIn
                .ToList()
                .ForEach(x =>
                    {
                        sb.Append($"BGP Peer '{this.Ipv4PeerAddress}' cannot be deleted because IP network " +
                        $"'{x.TenantIpNetwork.CidrNameIncludingIpv4LessThanOrEqualToLength}' is applied to the inbound policy of ");

                        if (x.AttachmentSet != null)
                        {
                            sb.Append($"attachment set '{x.AttachmentSet.Name}'.").Append("\r\n");
                        }
                        else
                        {
                            sb.Append($"tenant domain device '{x.BgpPeer.RoutingInstance.Device.Name}'.").Append("\r\n");
                        }
                    }
                );

            this.VpnTenantIpNetworksOut
                .ToList()
                .ForEach(x =>
                    {
                        sb.Append($"BGP Peer '{this.Ipv4PeerAddress}' cannot be deleted because IP network " +
                        $"'{x.TenantIpNetwork.CidrNameIncludingIpv4LessThanOrEqualToLength}' is applied to the outbound policy of ");

                        if (x.AttachmentSet != null)
                        {
                            sb.Append($"attachment set '{x.AttachmentSet.Name}'.").Append("\r\n");
                        }
                        else
                        {
                            sb.Append($"tenant domain device '{x.BgpPeer.RoutingInstance.Device.Name}'.").Append("\r\n");
                        }
                    }
                );

            if (sb.Length > 0) throw new IllegalDeleteAttemptException(sb.ToString());
        }
    }
}