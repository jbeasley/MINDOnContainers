using System;
using System.Net;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Services.Attachment.Domain.DomainModels.AttachmentAggregate
{
    public class BgpPeer : Entity
    {
        private string _ipv4PeerAddress;
        private int _peer2ByteAutonomousSystemNumber;
        private int? _maximumRoutes;
        private bool _isBfdEnabled;
        private bool _isMultiHopEnabled;
        private string _peerPassword;
        private readonly int _routingInstanceId;
        private readonly RoutingInstance _routingInstance;

        public BgpPeer(RoutingInstance routingInstance, string ipv4PeerAddress, int peer2ByteAutonomousSystemNumber, string peerPassword,
            int maximumRoutes = 500, bool isBfdEnabled = true, bool isMultiHopEnabled = false)
        {
            _routingInstance = routingInstance ?? throw new ArgumentNullException(nameof(routingInstance));
            _routingInstanceId = routingInstance.Id;

            SetIpv4PeerAddress(ipv4PeerAddress);
            SetPeerPassword(peerPassword);
            SetMaximumRoutes(maximumRoutes);
            SetPeer2ByteAutonomousSystemNumber(peer2ByteAutonomousSystemNumber);
            SetIsBfdEnabled(isBfdEnabled);
            SetIsMultiHopEnabled(isMultiHopEnabled);          
        }

        public void SetIpv4PeerAddress(string ipv4PeerAddress)
        {
            if (string.IsNullOrEmpty(ipv4PeerAddress)) throw new ArgumentNullException(nameof(ipv4PeerAddress));

            if (IPAddress.TryParse(ipv4PeerAddress, out IPAddress ipAddress))
            {
                CheckPeerAddressReachability(ipAddress);
                _ipv4PeerAddress = ipAddress.ToString();
            }
            else
            {
                throw new AttachmentDomainException($"'{ipv4PeerAddress}' is not valid.");
            }
        }

        public void SetPeer2ByteAutonomousSystemNumber(int peer2ByteAutonomousSystemNumber)
        {
            if (peer2ByteAutonomousSystemNumber < 1 || peer2ByteAutonomousSystemNumber > 65535)
            {
                throw new AttachmentDomainException("The peer 2 byte autonomous system number must be between 1 and 65535.");
            }

            this._peer2ByteAutonomousSystemNumber = peer2ByteAutonomousSystemNumber;
        }

        public void SetPeerPassword(string peerPassword)
        {
            if (string.IsNullOrEmpty(peerPassword)) throw new ArgumentNullException(nameof(peerPassword));
            if (peerPassword.Length < 8 || peerPassword.Length > 20)
            {
                throw new AttachmentDomainException("The peer password length must be between 8 and 20 characters.");
            }

            _peerPassword = peerPassword;
        }

        public void SetMaximumRoutes(int maximumRoutes)
        {
            if (maximumRoutes < 1 || maximumRoutes > 1000)
            {
                throw new AttachmentDomainException("Maximum routes must be between 1 and 1000.");
            }

            _maximumRoutes = maximumRoutes;
        }

        public void SetIsBfdEnabled(bool isBfdEnabled = true) => _isBfdEnabled = isBfdEnabled;

        public void SetIsMultiHopEnabled(bool isMultiHopEnabled = false) => _isMultiHopEnabled = isMultiHopEnabled;

        /// <summary>
        /// Validate the reachability of the peer address.
        /// </summary>
        private void CheckPeerAddressReachability(IPAddress ipv4PeerAddress) 
        {
            // For non-multihop peers, the peer IP address must be reachable from at least one vif or attachment which
            // belongs to the routing instance
            if (!this._isMultiHopEnabled)
            {
                var vlan = this._routingInstance.Vifs?.SelectMany(
                    vif =>
                    vif.Vlans)
                     .ToList()
                     .FirstOrDefault(
                        v =>
                        {
                            var ipv4 = v.GetIpv4Address();
                            return IPNetwork.TryParse(ipv4.Ipv4Address, ipv4.Ipv4SubnetMask, out IPNetwork network) && 
                                network.Contains(ipv4PeerAddress);
                        });

                var @interface = this._routingInstance.Attachments?.SelectMany(
                    attachment =>
                    attachment.Interfaces)
                     .ToList()
                     .FirstOrDefault(
                        i =>
                        {
                            var ipv4 = i.GetIpv4Address();
                            return IPNetwork.TryParse(ipv4.Ipv4Address, ipv4.Ipv4SubnetMask, out IPNetwork network) && 
                                network.Contains(ipv4PeerAddress);
                        });

                if (vlan == null && @interface == null)
                    throw new AttachmentDomainException($"The peer address '{this._ipv4PeerAddress}' is not contained by any network which is " +
                        $"directly reachable from routing instance '{this._routingInstance.Name}'. Check that the IP address for at least one vif or " +
                        $"attachment belonging to the routing instance is in the same IPv4 network as the bgp peer.");
            }
        }
    }
}