using System.Collections.Generic;
using System.Linq;
using MINDOnContainers.Services.Attachment.Domain.SeedWork;
using MINDOnContainers.Services.Attachment.Domain.Exceptions;

namespace MINDOnContainers.Vif.API.DomainModels.Vif
{
    public class Vif : Entity
    {
        private bool _isLayer3;
        [Range(2, 4094)]
        private int _vlanTag;
        private int? _routingInstanceId;
        private int? tenantID;
        private int? contractBandwidthPoolId;
        private int vifRoleId;
        private int? VlanTagRangeId;
        private bool created;
        private NetworkStatus _networkStatus;
        private int _mtuId;
        private virtual List<Vlan> _vlans;
        public IReadOnlyCollection<Vlan> Vlans => _vlans;

        public Vif(bool isLayer3, VifRole vifRole, int? vlanTag, int? routingInstanceID, int? tenantId, int? contractBandwidthPoolId, int? vlanTagRangeId, int mtuId)
        {
            _isLayer3 = isLayer3;

            if (vlanTag.HasValue)
            {
                if (vlanTag < 2 || vlanTag > 4094)
                {
                    throw new AttachmentDomainException("The vlan tag must be between 2 and 4094.");
                }
            }
            _vlanTag = vlanTag;



        }

        public void AddVlan(string ipv4Address, string ipv4SubnetMask)
        {
            if (_isLayer3)
            {
                // A layer 3 vif must have IP address and subnet supplied for each of its interfaces
                if (string.IsNullOrEmpty(ipv4Address))
                {
                    throw new AttachmentDomainException("A vlan for a layer 3 vif must be given an IPv4 address.");
                }

                if (string.IsNullOrEmpty(ipv4SubnetMask))
                {
                    throw new AttachmentDomainException("A vlan for a layer 3 vif must be given an IPv4 subnet mask.");
                }
            }
            else
            {   // A non-layer vif must NOT have IP address and subnet supplied for any of its interfaces
                if (!string.IsNullOrEmpty(ipv4Address))
                {
                    throw new AttachmentDomainException("A vlan for a layer 3 vif must NOT be given an IPv4 address.");
                }

                if (!string.IsNullOrEmpty(ipv4SubnetMask))
                {
                    throw new AttachmentDomainException("A vlan for a layer 3 vif must NOT be given an IPv4 subnet mask.");
                }
            }

            _vlans.Add(new Vlan(ipv4Address, ipv4SubnetMask));
        }

        /// <summary>
        /// Validate the state of the vif
        /// </summary>
        public virtual void Validate()
        {
            if (this.Attachment == null) throw new IllegalStateException("An attachment is required for the vif.");
            if (this.Mtu == null) throw new IllegalStateException("An MTU is required for the vif.");
            if (this.VifRole == null) throw new IllegalStateException("A vif role is required for the vif.");
            if (this.Vlans.Count != this.Attachment.Interfaces.Count) throw new IllegalStateException($"{this.Attachment.Interfaces.Count} vlans are required " +
                $"but only {this.Vlans.Count} were found. One vlan is required for each interface configured " +
                "for the attachment.");

            if (this.Attachment.AttachmentRole.PortPool.PortRole.PortRoleType == PortRoleTypeEnum.TenantFacing && this.Tenant == null)
            {
                throw new IllegalStateException("A tenant association is required for the vif in accordance with the vif role of " +
                    $"'{this.VifRole.Name}'.");
            }
            else if (this.VifRole.AttachmentRole.PortPool.PortRole.PortRoleType == PortRoleTypeEnum.ProviderInfrastructure && this.Tenant != null)
            {
                throw new IllegalStateException("A tenant association exists for the vif but is NOT required in accordance with the " +
                    $"vif role of '{this.VifRole.Name}'.");
            }

            if (this.IsLayer3)
            {
                if (this.Vlans.Count(x => !string.IsNullOrEmpty(x.IpAddress) && !string.IsNullOrEmpty(x.SubnetMask)) != this.Vlans.Count)
                {
                    throw new IllegalStateException("The vif is enabled for layer 3 but insufficient IPv4 addresses have been requested.");
                }
            }
            else if (this.Vlans.Any(x => !string.IsNullOrEmpty(x.IpAddress) || !string.IsNullOrEmpty(x.SubnetMask)))
            {
                throw new IllegalStateException("The vif is NOT enabled for layer 3 but IPv4 addresses have been requested.");
            }

            if (this.RoutingInstance == null && this.VifRole.RoutingInstanceTypeID.HasValue)
                throw new IllegalStateException("Illegal routing instance state. A routing instance for the vif is required in accordance " +
                    $"with the requested vif role of '{this.VifRole.Name}' but was not found.");

            if (this.RoutingInstance != null && !this.VifRole.RoutingInstanceTypeID.HasValue)
                throw new IllegalStateException("Illegal routing instance state. A routing instance for the vif has been assigned but is " +
                    $"not required for a vif with vif role of '{this.VifRole.Name}'.");

            if (this.RoutingInstance != null && this.VifRole.RoutingInstanceType != null)
            {
                if (this.RoutingInstance.RoutingInstanceType.RoutingInstanceTypeID != this.VifRole.RoutingInstanceTypeID)
                {
                    throw new IllegalStateException("Illegal routing instance state. The routing instance type for the vif is different to that " +
                        $"required by the vif role. The routing instance type required is '{this.VifRole.RoutingInstanceType.Type.ToString()}'. " +
                        $"The routing instance type assigned to the vif is '{this.RoutingInstance.RoutingInstanceType.Type.ToString()}'.");
                }
            }

            if (this.VifRole.RequireContractBandwidth)
            {
                if (this.ContractBandwidthPool == null)
                {
                    throw new IllegalStateException("A contract bandwidth for the vif is required in accordance with the vif role " +
                        $"of '{this.VifRole.Name}' but none is defined.");
                }

                // Bear in mind that if a new VIF has been created it will not appear in the VIFs collection of the Attachment entity
                // This is why we check to exclude any VIF with the same ID as this VIF, and then add the contract bandwidth of this VIF
                // to the aggregate bandwidth
                var aggContractBandwidthMbps = this.Attachment.Vifs
                                                              .Where(
                                                                vif => 
                                                                vif.VifID != this.VifID)
                                                              .Select(
                                                                vif =>
                                                                vif.ContractBandwidthPool.ContractBandwidth.BandwidthMbps)
                                                              .Aggregate(0, (x, y) => x + y) + this.ContractBandwidthPool.ContractBandwidth.BandwidthMbps;

                var attachmentBandwidthMbps = this.Attachment.AttachmentBandwidth.BandwidthGbps * 1000;
                if (attachmentBandwidthMbps < aggContractBandwidthMbps)
                {
                    throw new IllegalStateException($"The vif contract bandwidth of " +
                        $"{this.ContractBandwidthPool.ContractBandwidth.BandwidthMbps} Mbps is greater " +
                        $"than the remaining available bandwidth of the attachment " +
                        $"({attachmentBandwidthMbps - aggContractBandwidthMbps + this.ContractBandwidthPool.ContractBandwidth.BandwidthMbps } Mbps).");
                }
            }
            else
            {
                if (this.ContractBandwidthPool != null)
                {
                    throw new IllegalStateException("A contract bandwidth for the vif is defined but is NOT required for the vif role " +
                        $"of '{this.VifRole.Name}'.");
                }
            }
        }
    }
}