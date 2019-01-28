using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;
using Microsoft.EntityFrameworkCore;
using Mind.Models;
using IO.NovaAttSwagger.Model;
using System.Text;

namespace SCM.Models
{
    /// <summary>
    /// Attachment nova client dto extensions.
    /// </summary>
    public static class AttachmentNovaClientDtoExtensions {

        /// <summary>
        /// Create an instance of the nova tagged attachment dto.
        /// </summary>
        /// <returns>The nova tagged attachment dto.</returns>
        /// <param name="attachment">An instance of Attachment</param>
        public static DataAttachmentAttachmentPePePeName ToNovaTaggedAttachmentDto(this Attachment attachment)
        {
            // Create the dto only for tagged non-bundle, non-multiport attachments - currently we don't support
            // network updates for anything else!
            if (attachment.IsBundle || attachment.IsMultiPort || !attachment.IsTagged) return null;

            var vifs = (from vif in attachment.Vifs
                        from vlan in vif.Vlans
                        select new DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidVifVifvlanidAttachmentvif
                        {
                            VlanId = vif.VlanTag,
                            VrfName = vif.RoutingInstance.Name,
                            ContractBandwidthPoolName = vif.ContractBandwidthPool.Name,
                            EnableIpv4 = vif.RoutingInstance.RoutingInstanceType.IsLayer3.ToString().ToLower(),
                            Ipv4 = new DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidVifVifvlanidIpv4Attachmentipv4
                            {
                                Ipv4Address = vlan.IpAddress,
                                Ipv4SubnetMask = vlan.SubnetMask
                            }
                        }).ToList();

            var contractBandwidthPools = (from vif in attachment.Vifs
                                          select new DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidContractbandwidthpoolContractbandwidthpoolnameAttachmentcontractbandwidthpool
                                          {
                                              Name = vif.ContractBandwidthPool.Name,
                                              ContractBandwidth = Enum.Parse<DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidContractbandwidthpoolContractbandwidthpoolnameAttachmentcontractbandwidthpool
                                                                      .ContractBandwidthEnum>(vif.ContractBandwidthPool.ContractBandwidth.BandwidthMbps.ToString()),
                                              TrustReceivedCosAndDscp = vif.ContractBandwidthPool.TrustReceivedCosAndDscp.ToString().ToLower(),
                                              ServiceClasses = new List<DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameAttachmentserviceclasses>
                                              {
                                                new DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameAttachmentserviceclasses
                                                {
                                                    ScName = DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameAttachmentserviceclasses.ScNameEnum.SIGMA20MarketDataTCP,
                                                    ScBandwidth =  new DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameScbandwidthAttachmentscbandwidth
                                                    {
                                                        BwUnits = DataAttachmentContractbandwidthpoolContractbandwidthpoolnameServiceclassesServiceclassesscnameScbandwidthAttachmentscbandwidth.BwUnitsEnum.Mbps,
                                                        Bandwidth = vif.ContractBandwidthPool.ContractBandwidth.BandwidthMbps,
                                                        BurstSize = CalculateBurstSizeBytes(vif.ContractBandwidthPool.ContractBandwidth.BandwidthMbps)
                                                    }
                                                }
                                            }
                                          }).ToList();

            var bgpPeers = (from vif in attachment.Vifs
                            from bgpPeer in vif.RoutingInstance.BgpPeers
                            select new DataAttachmentAttachmentPePepenameVrfVrfvrfnameBgppeerBgppeerpeeripv4addressAttachmentbgppeer
                            {
                                PeerIpv4Address = bgpPeer.Ipv4PeerAddress,
                                PeerPassword = bgpPeer.PeerPassword,
                                PeerAutonomousSystem = bgpPeer.Peer2ByteAutonomousSystem,
                                IsBfdEnabled = bgpPeer.IsBfdEnabled.ToString().ToLower(),
                                IsMultiHop = bgpPeer.IsMultiHop.ToString().ToLower(),
                                MaxPeerRoutes = bgpPeer.MaximumRoutes
                            }).ToList();

            var vrfs = (from vif in attachment.Vifs
                        select new DataAttachmentAttachmentPePepenameVrfVrfvrfnameAttachmentvrf
                        {
                            VrfName = vif.RoutingInstance.Name,
                            RdAdministratorSubfield = vif.RoutingInstance.AdministratorSubField,
                            RdAssignedNumberSubfield = vif.RoutingInstance.AssignedNumberSubField,
                            BgpPeer = bgpPeers
                        }).ToList();

            var data = new DataAttachmentAttachmentPePePeName
            {
                Attachmentpe = new List<DataAttachmentAttachmentPePepenameAttachmentpe>
                {
                    new DataAttachmentAttachmentPePepenameAttachmentpe
                    {
                        PeName = attachment.Device.Name,
                        Vrf = vrfs,
                        TaggedAttachmentInterface = new List<DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidAttachmenttaggedattachmentinterface>
                        {
                            new DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidAttachmenttaggedattachmentinterface
                            {
                                AttachmentBandwidth = Enum.Parse<DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidAttachmenttaggedattachmentinterface
                                                          .AttachmentBandwidthEnum>(attachment.AttachmentBandwidth.BandwidthGbps.ToString()),
                                InterfaceId = attachment.PortName,
                                InterfaceType = Enum.Parse<DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidAttachmenttaggedattachmentinterface
                                                    .InterfaceTypeEnum>(attachment.PortType),
                                InterfaceMtu = Enum.Parse<DataAttachmentAttachmentPePepenameTaggedattachmentinterfaceTaggedattachmentinterfaceinterfacetypeTaggedattachmentinterfaceinterfaceidAttachmenttaggedattachmentinterface
                                                   .InterfaceMtuEnum>(attachment.Mtu.MtuValue.ToString()),
                                 Vif = vifs,
                                 ContractBandwidthPool = contractBandwidthPools
                            }
                        }
                    }
                }
            };

            return data;
        }

        /// <summary>
        /// Calculates the burst size bytes.
        /// </summary>
        /// <returns>The burst size bytes.</returns>
        /// <param name="contractBandwidthMbps">Contract bandwidth mbps.</param>
        private static int CalculateBurstSizeBytes(double contractBandwidthMbps)
        {
            double bc = 0.05;
            var burstBytes = (contractBandwidthMbps * 1000000 * bc) / 8;
            return Convert.ToInt32(burstBytes);
        }
    }

    public static class AttachmentQueryableExtensions
    {
        public static IQueryable<Attachment> IncludeValidationProperties(this IQueryable<Attachment> query)
        {
            return query.Include(x => x.Tenant)
                        .Include(x => x.Device.DeviceRole.DeviceRoleAttachmentRoles)
                        .Include(x => x.Device.RoutingInstances)
                        .Include(x => x.RoutingInstance.Attachments)
                        .Include(x => x.RoutingInstance.Vifs)
                        .Include(x => x.RoutingInstance.RoutingInstanceType)
                        .Include(x => x.RoutingInstance.RouteDistinguisherRange)
                        .Include(x => x.ContractBandwidthPool)
                        .Include(x => x.AttachmentRole.PortPool.PortRole)
                        .Include(x => x.AttachmentBandwidth)
                        .Include(x => x.Interfaces)
                        .ThenInclude(x => x.Ports)
                        .ThenInclude(x => x.PortBandwidth)
                        .Include(x => x.Mtu)
                        .Include(x => x.Vifs)
                        .ThenInclude(x => x.ContractBandwidthPool.ContractBandwidth)
                        .Include(x => x.Vifs)
                        .ThenInclude(x => x.Vlans)
                        .Include(x => x.Vifs)
                        .ThenInclude(x => x.RoutingInstance.RoutingInstanceType)
                        .Include(x => x.Vifs)
                        .ThenInclude(x => x.RoutingInstance.BgpPeers)
                        .Include(x => x.Vifs)
                        .ThenInclude(x => x.RoutingInstance.AttachmentSetRoutingInstances)
                        .ThenInclude(x => x.AttachmentSet.VpnAttachmentSets)
                        .ThenInclude(x => x.Vpn);
        }

        public static IQueryable<Attachment> IncludeDeleteValidationProperties(this IQueryable<Attachment> query)
        {
            return query.Include(x => x.Device)
                                     .Include(x => x.AttachmentRole.PortPool.PortRole)
                                     .Include(x => x.ContractBandwidthPool.Attachments)
                                     .Include(x => x.ContractBandwidthPool.Vifs)
                                     .Include(x => x.Interfaces)
                                     .ThenInclude(x => x.Ports)
                                     .ThenInclude(x => x.PortStatus)
                                     .Include(x => x.Vifs)
                                     .ThenInclude(x => x.Vlans)
                                     .Include(x => x.Vifs)
                                     .ThenInclude(x => x.RoutingInstance.RoutingInstanceType)
                                     .Include(x => x.Vifs)
                                     .ThenInclude(x => x.RoutingInstance.Attachments)
                                     .Include(x => x.Vifs)
                                     .ThenInclude(x => x.RoutingInstance.Vifs)
                                     .Include(x => x.Vifs)
                                     .ThenInclude(x => x.ContractBandwidthPool.Vifs)
                                     .Include(x => x.Vifs)
                                     .ThenInclude(x => x.ContractBandwidthPool.Attachments)
                                     .Include(x => x.Vifs)
                                     .ThenInclude(x => x.RoutingInstance.AttachmentSetRoutingInstances)
                                     .ThenInclude(x => x.AttachmentSet.VpnAttachmentSets)
                                     .ThenInclude(x => x.Vpn)
                                     .Include(x => x.RoutingInstance.RoutingInstanceType)
                                     .Include(x => x.RoutingInstance.Vifs)
                                     .Include(x => x.RoutingInstance.Attachments)
                                     .Include(x => x.RoutingInstance.BgpPeers)
                                     .Include(x => x.RoutingInstance.AttachmentSetRoutingInstances)
                                     .ThenInclude(x => x.AttachmentSet.VpnAttachmentSets)
                                     .ThenInclude(x => x.Vpn);
        }

        public static IQueryable<Attachment> IncludeDeepProperties(this IQueryable<Attachment> query)
        {
            return query.Include(x => x.Device.Location.SubRegion.Region)
                        .Include(x => x.Device.Plane)
                        .Include(x => x.ContractBandwidthPool.ContractBandwidth)
                        .Include(x => x.Interfaces)
                        .ThenInclude(x => x.Ports)
                        .Include(x => x.Mtu)
                        .Include(x => x.RoutingInstance.BgpPeers)
                        .Include(x => x.RoutingInstance.LogicalInterfaces)
                        .Include(x => x.Tenant)
                        .Include(x => x.AttachmentRole)
                        .Include(x => x.AttachmentBandwidth);
        }
     }

    public class Attachment : IModifiableResource, IEquatable<Attachment>
    {
        [Key]
        public int AttachmentID { get; private set; }

        /// <summary>
        /// Gets the name of the attachment.
        /// </summary>
        /// <value>String value denoting the name of the attachment</value>
        [NotMapped]
        public string Name
        {
            get
            {
                if (IsBundle)
                {
                    return $"Bundle{ID}";
                }
                else if (IsMultiPort)
                {
                    return $"MultiPort{ID}";
                }
                else
                {
                    var port = Interfaces?.SingleOrDefault()?.Ports?.SingleOrDefault();
                    if (port != null) return $"{port.Type} {port.Name}";
                    return string.Empty;
                }
            }
        }
        /// <summary>
        /// Convenience method - gets the type of the underlying port for single attachments.
        /// For bundle or multiport attachments this method returns an empty string.
        /// </summary>
        /// <value>String denoting the name of port type</value>
        [NotMapped]
        public string PortType {
            get {
                if (!IsBundle && !IsMultiPort)
                {

                    var port = Interfaces?.SingleOrDefault()?.Ports?.SingleOrDefault();
                    if (port != null) return port.Type;
                }
                return string.Empty;
            }
        }
        /// <summary>
        /// Convenience method - gets the name of the underlying port for single attachments.
        /// For bundle or multiport attachments this method returns an empty string.
        /// </summary>
        /// <value>String denoting the name of port name</value>
        [NotMapped]
        public string PortName
        {
            get
            {
                if (!IsBundle && !IsMultiPort)
                {

                    var port = Interfaces?.SingleOrDefault()?.Ports?.SingleOrDefault();
                    if (port != null) return port.Name;
                }
                return string.Empty;
            }
        }
        [MaxLength(250)]
        public string Description { get; set; }
        [MaxLength(250)]
        public string Notes { get; set; }
        public bool IsTagged { get; set; }
        public bool IsLayer3 { get; set; }
        public bool IsBundle { get; set; }
        public int? BundleMinLinks { get; set; }
        public int? BundleMaxLinks { get; set; }
        public bool IsMultiPort { get; set; }
        public int? ID { get; set; }
        public int AttachmentBandwidthID { get; set; }
        public int? TenantID { get; set; }
        public int DeviceID { get; set; }
        public int? RoutingInstanceID { get; set; }
        public int? ContractBandwidthPoolID { get; set; }
        public int AttachmentRoleID { get; set; }
        public int MtuID { get; set; }
        public bool Created { get; set; }
        public bool ShowCreatedAlert { get; set; }
        public NetworkStatusEnum NetworkStatus { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        [ForeignKey("TenantID")]
        public virtual Tenant Tenant { get; set; }
        public virtual Device Device { get; set; }
        public virtual RoutingInstance RoutingInstance { get; set; }
        public virtual AttachmentBandwidth AttachmentBandwidth { get; set; }
        [ForeignKey("ContractBandwidthPoolID")]
        public virtual ContractBandwidthPool ContractBandwidthPool { get; set; }
        [ForeignKey("MtuID")]
        public virtual Mtu Mtu { get; set; }
        public virtual AttachmentRole AttachmentRole { get; set; }
        public virtual ICollection<Interface> Interfaces { get; set; }
        public virtual ICollection<Vif> Vifs { get; set; }
        string IModifiableResource.ConcurrencyToken => this.GetWeakETag();

        public bool Equals(Attachment obj)
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
                // Suitable nullity checks etc, of course :)

                hashCode = hashCode * 59 + AttachmentID.GetHashCode();
                hashCode = hashCode * 59 + TenantID.GetHashCode();
                hashCode = hashCode * 59 + DeviceID.GetHashCode();
                hashCode = hashCode * 59 + AttachmentBandwidthID.GetHashCode();
                hashCode = hashCode * 59 + RoutingInstanceID.GetHashCode();
                hashCode = hashCode * 59 + ContractBandwidthPoolID.GetHashCode();
                hashCode = hashCode * 59 + AttachmentRoleID.GetHashCode();
                hashCode = hashCode * 59 + MtuID.GetHashCode();
                hashCode = hashCode * 59 + IsLayer3.GetHashCode();
                hashCode = hashCode * 59 + Created.GetHashCode();
                hashCode = hashCode * 59 + ShowCreatedAlert.GetHashCode();
                hashCode = hashCode * 59 + IsBundle.GetHashCode();
                hashCode = hashCode * 59 + BundleMaxLinks.GetHashCode();
                hashCode = hashCode * 59 + IsMultiPort.GetHashCode();
                hashCode = hashCode * 59 + IsTagged.GetHashCode();
                hashCode = hashCode * 59 + NetworkStatus.GetHashCode();
                if (BundleMinLinks != null)
                    hashCode = hashCode * 59 + BundleMinLinks.GetHashCode();
                if (BundleMaxLinks != null)
                    hashCode = hashCode * 59 + BundleMaxLinks.GetHashCode();
                if (Tenant != null)
                    hashCode = hashCode * 59 + Tenant.GetHashCode();
                if (Device != null)
                    hashCode = hashCode * 59 + Device.GetHashCode();
                if (AttachmentBandwidth != null)
                    hashCode = hashCode * 59 + AttachmentBandwidth.GetHashCode();
                if (ContractBandwidthPool != null)
                    hashCode = hashCode * 59 + ContractBandwidthPool.GetHashCode();
                if (RoutingInstance != null)
                    hashCode = hashCode * 59 + RoutingInstance.GetHashCode();
                if (Mtu != null)
                    hashCode = hashCode * 59 + Mtu.GetHashCode();
                if (AttachmentRole != null)
                    hashCode = hashCode * 59 + AttachmentRole.GetHashCode();
                if (Interfaces != null)
                    hashCode = hashCode * 59 + Interfaces.GetHashCode();
                if (Vifs != null)
                    hashCode = hashCode * 59 + Vifs.GetHashCode();
                return hashCode;
            }
        }


        /// <summary>
        /// Validate the state of the attachment.
        /// </summary>
        public virtual void Validate()
        {
            if (this.Mtu == null) throw new IllegalStateException("An MTU is required for the attachment.");
            if (this.AttachmentBandwidth == null) throw new IllegalStateException("An attachment bandwidth is required for the attachment.");
            if (this.AttachmentRole == null) throw new IllegalStateException("An attachment role is required for the attachment.");
            if (this.Device == null) throw new IllegalStateException("A device is required for the attachment.");
            if (!this.Interfaces.Any()) throw new IllegalStateException("At least one interface is required for the attachment.");
            if (!this.Device.DeviceRole.DeviceRoleAttachmentRoles
                .Any(
                    x => 
                    x.AttachmentRoleID == this.AttachmentRole.AttachmentRoleID))
            {
                throw new IllegalStateException($"The attachment role of '{this.AttachmentRole.Name}' is not valid for device '{this.Device.Name}' because " +
                    $"the device is assigned to role '{this.Device.DeviceRole.Name}'.");
            }
            if (this.AttachmentRole.PortPool.PortRole.PortRoleType == PortRoleTypeEnum.TenantFacing && this.Tenant == null)
            {
                throw new IllegalStateException("A tenant association is required for the attachment in accordance with the attachment role of " +
                    $"'{this.AttachmentRole.Name}'.");
            }
            else if (this.AttachmentRole.PortPool.PortRole.PortRoleType == PortRoleTypeEnum.ProviderInfrastructure && this.Tenant != null)
            {
                throw new IllegalStateException("A tenant association exists for the attachment but is NOT required in accordance with the " +
                    $"attachment role of '{this.AttachmentRole.Name}'.");
            }

            if (this.RoutingInstance == null && this.AttachmentRole.RoutingInstanceTypeID.HasValue)
                throw new IllegalStateException("Illegal routing instance state. A routing instance for the attachment is required in accordance " +
                    $"with the requested attachment role of '{this.AttachmentRole.Name}' but was not found.");

            if (this.RoutingInstance != null && !this.AttachmentRole.RoutingInstanceTypeID.HasValue)
                throw new IllegalStateException("Illegal routing instance state. A routing instance for the attachment has been assigned but is " +
                    $"not required for an attachment with attachment role of '{this.AttachmentRole.Name}'.");

            if (this.RoutingInstance != null && this.AttachmentRole.RoutingInstanceType != null)
            {
                if (this.RoutingInstance.RoutingInstanceType.RoutingInstanceTypeID != this.AttachmentRole.RoutingInstanceTypeID)
                {
                    throw new IllegalStateException("Illegal routing instance state. The routing instance type for the attachment is different to that " +
                        $"required by the attachment role. The routing instance type required is '{this.AttachmentRole.RoutingInstanceType.Type.ToString()}'. " +
                        $"The routing instance type assigned to the attachment is '{this.RoutingInstance.RoutingInstanceType.Type.ToString()}'.");
                }
            }

            if (this.IsLayer3)
            {
                if (this.Interfaces.Count(x => !string.IsNullOrEmpty(x.IpAddress) &&
                !string.IsNullOrEmpty(x.SubnetMask)) != this.Interfaces.Count)
                {
                    throw new IllegalStateException("The attachment is enabled for layer 3 but insufficient IPv4 addresses have been requested.");
                }
            }
            else if (this.Interfaces.Where(x => !string.IsNullOrEmpty(x.IpAddress) || !string.IsNullOrEmpty(x.SubnetMask)).Any())
            {
                throw new IllegalStateException("The attachment is NOT enabled for layer 3 but IPv4 addresses have been requested.");
            }

            if (this.AttachmentRole.RequireContractBandwidth)
            {
                if (this.ContractBandwidthPool?.ContractBandwidth == null )
                {
                    throw new IllegalStateException("A contract bandwidth for the attachment is required in accordance with the attachment role " +
                        $"of '{this.AttachmentRole.Name}' but none is defined.");
                }

                if (this.ContractBandwidthPool.ContractBandwidth.BandwidthMbps > this.AttachmentBandwidth.BandwidthGbps * 1000)
                {
                    throw new IllegalStateException($"The requested contract bandwidth of " +
                        $"{this.ContractBandwidthPool.ContractBandwidth.BandwidthMbps} Mbps is greater " +
                        $"than the bandwidth of the attachment which is {this.AttachmentBandwidth.BandwidthGbps} Gbps.");
                }
            }
            else
            {
                if (this.ContractBandwidthPool != null)
                {
                    throw new IllegalStateException("A contract bandwidth for the attachment is defined but is NOT required for the attachment role " +
                        $"of '{this.AttachmentRole.Name}'.");
                }
            }

            if (this.AttachmentRole.IsTaggedRole)
            {
                if (!this.IsTagged)
                {
                    throw new IllegalStateException("The attachment must be enabled for tagging with the 'isTagged' property in accordance with the " +
                        $"attachment role of '{this.AttachmentRole.Name}'.");
                }
                if (this.IsLayer3) throw new IllegalStateException("Layer 3 cannot be enabled concurrently with a tagged attachment.");
            }
            else
            {
                if (this.Vifs.Any())
                {
                    throw new IllegalStateException("Vifs were found for the attachment but the attachment is not enabled for tagging with the 'isTagged' properrty.");
                }
            }

            if (this.IsBundle)
            {
                if (!this.AttachmentRole.SupportedByBundle) throw new IllegalStateException($"The requested attachment role " +
                $"'{this.AttachmentRole.Name}' is not supported with a bundle attachment.");

                if (!this.AttachmentBandwidth.SupportedByBundle) throw new IllegalStateException($"The requested attachment " +
                $"bandwidth '{this.AttachmentBandwidth.BandwidthGbps} Gbps' is not supported with a bundle attachment.");

                var numPorts = this.Interfaces.SelectMany(x => x.Ports).Count();
                if (this.BundleMinLinks > numPorts) throw new IllegalStateException($"The min links parameter for the bundle " +
                    $"({this.BundleMinLinks}) must be " +
                    $"less than or equal to the total number of ports required for the bundle ({numPorts}).");

                if (this.BundleMaxLinks > numPorts) throw new IllegalStateException($"The max links parameter for the bundle " +
                    $"({this.BundleMaxLinks}) must be " +
                    $"less than or equal to the total number of ports required for the bundle ({numPorts}).");

                if (this.BundleMinLinks > this.BundleMaxLinks) throw new IllegalStateException($"The min links parameter for the bundle " +
                    $"({this.BundleMinLinks}) must be less then " +
                    $"or equal to the max links parameter for the bundle ({this.BundleMaxLinks})");
            }

            if (this.IsMultiPort)
            {
                if (!this.AttachmentRole.SupportedByMultiPort) throw new IllegalStateException($"The requested attachment role " +
                    $"'{this.AttachmentRole.Name}' is not supported with a multiport attachment.");

                if (!this.AttachmentBandwidth.SupportedByMultiPort) throw new IllegalStateException($"The requested attachment " +
                    $"bandwidth '{this.AttachmentBandwidth.BandwidthGbps} Gbps' is not supported with a multiport attachment.");
            }

            // Validate the routing instance if one exists
            if (this.RoutingInstance != null)
            {
                this.RoutingInstance.Validate();
            }
        }

        /// <summary>
        /// Validates that the ports of a an attachment are configured correctly.
        /// </summary>
        public virtual void ValidatePortsConfiguredCorrectly()
        {
            var totalPortBandwidthGbps = (this.Interfaces
                                              .SelectMany(
                                                 x => 
                                                 x.Ports)
                                               .Where(
                                                 x => 
                                                 x.DeviceID == this.DeviceID)
                                               .Sum(
                                                 x => 
                                                 x.PortBandwidth.BandwidthGbps)
                                         );

            if (totalPortBandwidthGbps < this.AttachmentBandwidth.BandwidthGbps)
            {
                throw new IllegalStateException($"Attachment '{this.Name}' is misconfigured. The total port bandwidth "
                    + $"({totalPortBandwidthGbps} Gbps) is less than the required attachment bandwidth ({this.AttachmentBandwidth.BandwidthGbps} Gbps). "
                    + "Check that the correct number of ports are configured and that all of the ports are configured for the same device.");
            }
        }
    }
}