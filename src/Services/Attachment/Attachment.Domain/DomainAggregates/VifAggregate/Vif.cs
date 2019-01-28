using Microsoft.EntityFrameworkCore;
using Mind.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;

namespace MINDOnContainers.Vif.API.DomainModels
{

    public static class VifQueryableExtensions
    {
        public static IQueryable<Vif> IncludeValidationProperties(this IQueryable<Vif> query)
        {
            return query.Include(x => x.Attachment.Vifs)
                        .ThenInclude(x => x.ContractBandwidthPool.ContractBandwidth)
                        .Include(x => x.Attachment.AttachmentBandwidth)
                        .Include(x => x.Attachment.Device)
                        .Include(x => x.Attachment.Interfaces)
                        .ThenInclude(x => x.Ports)
                        .Include(x => x.Attachment.Mtu)
                        .Include(x => x.Vlans)
                        .Include(x => x.VifRole.AttachmentRole.PortPool.PortRole)
                        .Include(x => x.VifRole.RoutingInstanceType)
                        .Include(x => x.Tenant)
                        .Include(x => x.ContractBandwidthPool)
                        .Include(x => x.Mtu)
                        .Include(x => x.RoutingInstance.BgpPeers)
                        .Include(x => x.RoutingInstance.AttachmentSetRoutingInstances)
                        .ThenInclude(x => x.AttachmentSet.VpnAttachmentSets)
                        .ThenInclude(x => x.Vpn);
        }

        public static IQueryable<Vif> IncludeDeleteValidationProperties(this IQueryable<Vif> query)
        {
            return query.Include(x => x.Attachment.Device)
                        .Include(x => x.Attachment.Interfaces)
                        .ThenInclude(x => x.Ports)
                        .Include(x => x.RoutingInstance.Vifs)
                        .Include(x => x.RoutingInstance.Attachments)
                        .Include(x => x.ContractBandwidthPool)
                        .Include(x => x.Vlans)
                        .Include(x => x.RoutingInstance.RoutingInstanceType)
                        .Include(x => x.RoutingInstance.AttachmentSetRoutingInstances)
                        .ThenInclude(x => x.AttachmentSet.VpnAttachmentSets)
                        .ThenInclude(x => x.Vpn)
                        .Include(x => x.ContractBandwidthPool.Vifs)
                        .Include(x => x.ContractBandwidthPool.Attachments)
                        .Include(x => x.RoutingInstance.BgpPeers);
        }

        public static IQueryable<Vif> IncludeDeepProperties(this IQueryable<Vif> query)
        {
            return query.Include(x => x.VifRole)
                        .Include(x => x.Attachment.Interfaces)
                        .ThenInclude(x => x.Ports)
                        .Include(x => x.ContractBandwidthPool.ContractBandwidth)
                        .Include(x => x.RoutingInstance.BgpPeers)
                        .ThenInclude(x => x.VpnTenantIpNetworksIn)
                        .ThenInclude(x => x.TenantIpNetwork)
                        .Include(x => x.RoutingInstance.BgpPeers)
                        .ThenInclude(x => x.VpnTenantIpNetworksOut)
                        .ThenInclude(x => x.TenantIpNetwork)
                        .Include(x => x.RoutingInstance.LogicalInterfaces)
                        .Include(x => x.RoutingInstance.RoutingInstanceType)
                        .Include(x => x.Vlans)
                        .Include(x => x.Tenant)
                        .Include(x => x.Mtu);
        }
    }

    public class Vif : IModifiableResource, IEquatable<Vif>
    {
        public int VifID { get; private set; }
        public bool IsLayer3 { get; set; }
        public int AttachmentID { get; set; }
        [Range(2, 4094)]
        public int VlanTag { get; set; }
        public int? RoutingInstanceID { get; set; }
        public int? TenantID { get; set; }
        public int? ContractBandwidthPoolID { get; set; }
        public int VifRoleID { get; set; }
        public int? VlanTagRangeID { get; set; }
        public bool Created { get; set; }
        public NetworkStatusEnum NetworkStatus { get; set; }
        public int MtuID { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual ContractBandwidthPool ContractBandwidthPool { get; set; }
        public virtual VlanTagRange VlanTagRange { get; set; }
        public virtual VifRole VifRole { get; set; }
        public virtual Mtu Mtu { get; set; }
        public virtual ICollection<Vlan> Vlans { get; set; }
        string IModifiableResource.ConcurrencyToken => this.GetWeakETag();

        public bool Equals(Vif obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            return obj.GetType() == this.GetType() && this.Equals(obj);
        }

        /// <summary>
        /// Gets the hash code
        /// </summary>
        /// <returns>Hash code</returns>
        public override int GetHashCode()
        {
            unchecked // Overflow is fine, just wrap
            {
                var hashCode = 41;
                hashCode = hashCode * 59 + VifID.GetHashCode();
                hashCode = hashCode * 59 + IsLayer3.GetHashCode();
                hashCode = hashCode * 59 + AttachmentID.GetHashCode();
                hashCode = hashCode * 59 + VlanTag.GetHashCode();
                if (TenantID != null)
                hashCode = hashCode * 59 + TenantID.GetHashCode();
                if (ContractBandwidthPoolID != null) hashCode = hashCode * 59 + ContractBandwidthPoolID.GetHashCode();
                hashCode = hashCode * 59 + VifRoleID.GetHashCode();
                hashCode = hashCode * 59 + VlanTagRangeID.GetHashCode();
                if (Vlans != null) hashCode = hashCode * 59 + Vlans.GetHashCode();
                if (ContractBandwidthPool != null) hashCode = hashCode * 59 + ContractBandwidthPool.GetHashCode();
                if (Mtu != null) hashCode = hashCode * 59 + Mtu.GetHashCode();  
                hashCode = hashCode * 59 + VifRoleID.GetHashCode();
                hashCode = hashCode * 59 + NetworkStatus.GetHashCode();
                hashCode = hashCode * 59 + Created.GetHashCode();
                hashCode = hashCode * 59 + ShowCreatedAlert.GetHashCode();
                if (VifRole != null) hashCode = hashCode * 59 + VifRole.GetHashCode();
                if (Mtu != null) hashCode = hashCode * 59 + Mtu.GetHashCode();

                return hashCode;
            }
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