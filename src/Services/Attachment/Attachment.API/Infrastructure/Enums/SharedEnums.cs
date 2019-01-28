using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Threading.Tasks;

namespace Mind.Api
{
    /// <summary>
    /// Enum for the network plane options
    /// </summary>
    /// <plane>Enumerated list of network plane options</plane>
    public enum PlaneEnum
    {
        /// <summary>
        /// Enum for the red plane
        /// </summary>
        [EnumMember(Value = "red")]
        Red = 1,

        /// <summary>
        /// Enum for the blue plane
        /// </summary>
        [EnumMember(Value = "blue")]
        Blue = 2
    }

    /// <summary>
    /// Enum for the geographical region option
    /// </summary>
    /// <value>Enumerated list of region options</value>
    public enum RegionEnum
    {
        /// <summary>
        /// Enum for None
        /// </summary>
        [EnumMember(Value = "None")]
        None = 0,

        /// <summary>
        /// Enum for EMEA
        /// </summary>
        [EnumMember(Value = "EMEA")]
        EMEA = 1,

        /// <summary>
        /// Enum for ASIAPAC
        /// </summary>
        [EnumMember(Value = "ASIAPAC")]
        ASIAPAC = 2,

        /// <summary>
        /// Enum for AMERS
        /// </summary>
        [EnumMember(Value = "AMERS")]
        AMERS = 3
    }

    /// <summary>
    /// Enumeration of attachment redundancy level options
    /// </summary>
    /// <value>Enumerated list of attachment redundancy options</value>
    public enum AttachmentRedundancyEnum
    {
        /// <summary>
        /// Enum for Bronze
        /// </summary>
        [EnumMember(Value = "Bronze")]
        Bronze = 1,

        /// <summary>
        /// Enum for Silver
        /// </summary>
        [EnumMember(Value = "Silver")]
        Silver = 2,

        /// <summary>
        /// Enum for Gold
        /// </summary>
        [EnumMember(Value = "Gold")]
        Gold = 3,

        /// <summary>
        /// Enum for Custom
        /// </summary>
        [EnumMember(Value = "Custom")]
        Custom = 4
    }

    /// <summary>
    /// Enum for the tenancy type options of a vpn.
    /// </summary>
    /// <value>Enumerated list of tenancy type options</value>
    public enum TenancyTypeEnum
    {
        /// <summary>
        /// Enum for Single
        /// </summary>
        [EnumMember(Value = "single")]
        Single = 1,

        /// <summary>
        /// Enum for Multi
        /// </summary>
        [EnumMember(Value = "multi")]
        Multi = 2
    }

    /// <summary>
    /// Enum for the topology type options of a vpn.
    /// </summary>
    /// <value>Enumerated list of topology type options</value>
    public enum TopologyTypeEnum
    {
        /// <summary>
        /// Enum for Meshed
        /// </summary>
        [EnumMember(Value = "meshed")]
        Meshed = 1,

        /// <summary>
        /// Enum for Hub-and-Spoke
        /// </summary>
        [EnumMember(Value = "hubAndSpoke")]
        HubAndSpoke = 2
    }

    /// <summary>
    /// Enum for the address family options of a vpn. Currently only IPv4 is available. 
    /// </summary>
    /// <value>Enumerated list of address-family options</value>
    public enum AddressFamilyEnum
    {
        /// <summary>
        /// Enum for IPv4
        /// </summary>
        [EnumMember(Value = "IPv4")]
        IPv4 = 1
    }

    /// <summary>
    /// Enum for the multicast vpn service type options of a vpn.
    /// </summary>
    public enum MulticastVpnServiceTypeEnum
    {
        /// <summary>
        /// Enum for SSM
        /// </summary>
        [EnumMember(Value = "SSM")]
        SSM = 1
    }

    /// <summary>
    /// Enum for route target range names
    /// </summary>
    /// <value>Enumerated list of route target range options</value>
    public enum RouteTargetRangeEnum
    {
        /// <summary>
        /// Enum for Default
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 1,

        /// <summary>
        /// Enum for Sigma
        /// </summary>
        [EnumMember(Value = "sigma")]
        Sigma = 2
    }

    /// <summary>
    /// Enum for route distinguisher ranges
    /// </summary>
    /// <value>Enumerated list of route distinguisher range options</value>
    public enum RouteDistinguisherRangeTypeEnum
    {
        /// <summary>
        /// Enum for Default
        /// </summary>
        [EnumMember(Value = "default")]
        Default = 0
    }

    /// <summary>
    /// Enum for the multicast vpn direction type options of a vpn.
    /// </summary>
    /// <value>Enumerated list of multicast vpn direction type options</value>
    public enum MulticastVpnDirectionTypeEnum
    {
        /// <summary>
        /// Enum for Unidirectional
        /// </summary>
        [EnumMember(Value = "unidirectional")]
        Unidirectional = 1,

        /// <summary>
        /// Enum for Bidirectional
        /// </summary>
        [EnumMember(Value = "bidirectional")]
        Bidirectional = 2
    }

    /// <summary>
    /// Enumeration of multicast domain types supported by the attachment set
    /// </summary>
    /// <value>Enumerated list of multicast vpn domain type options</value>
    public enum MulticastVpnDomainTypeEnum
    {
        /// <summary>
        /// Enum for Sender-Only
        /// </summary>
        [EnumMember(Value = "Sender-Only")]
        SenderOnly = 1,

        /// <summary>
        /// Enum for Receiver-Only
        /// </summary>
        [EnumMember(Value = "Receiver-Only")]
        ReceiverOnly = 2,

        /// <summary>
        /// Enum for Sender-and-Receiver
        /// </summary>
        [EnumMember(Value = "Sender-and-Receiver")]
        SenderAndReceiver = 3
    }

    /// <summary>
    /// Enumeration of tenant IP routing behaviour options
    /// </summary>
    /// <value>Enumerated list of tenant ip routing behaviour options</value>
    public enum TenantIpRoutingBehaviourEnum
    {
        /// <summary>
        /// Enum for Any-Plane
        /// </summary>
        [EnumMember(Value = "Any-Plane")]
        AnyPlane = 1,

        /// <summary>
        /// Enum for Red-Plane
        /// </summary>
        [EnumMember(Value = "Red-Plane")]
        RedPlane = 2,

        /// <summary>
        /// Enum for Blue-Plane
        /// </summary>
        [EnumMember(Value = "Blue-Plane")]
        BluePlane = 3
    }

    /// <summary>
    /// Enumeration for device status options
    /// </summary>
    public enum DeviceStatusTypeEnum
    {
        /// <summary>
        /// Enum for Production
        /// </summary>
        [EnumMember(Value ="Production")]
        Production = 0,

        /// <summary>
        /// Enum for Staging
        /// </summary>
        [EnumMember(Value ="Staging")]
        Staging = 1,

        /// <summary>
        /// Enum for Retired
        /// </summary>
        [EnumMember(Value = "Retired")]
        Retired = 2
    }

    /// <summary>
    /// Enumeration for port status options
    /// </summary>
    public enum PortStatusTypeEnum
    {
        /// <summary>
        /// Enum for Free
        /// </summary>
        [EnumMember(Value = "Free")]
        Free = 0,

        /// <summary>
        /// Enum for Assigned
        /// </summary>
        [EnumMember(Value = "Assigned")]
        Assigned = 1,

        /// <summary>
        /// Enum for Locked
        /// </summary>
        [EnumMember(Value = "Locked")]
        Locked = 2,

        /// <summary>
        /// Enum for Migration
        /// </summary>
        [EnumMember(Value = "Migration")]
        Migration = 3,

        /// <summary>
        /// Enum for Reserved
        /// </summary>
        [EnumMember(Value = "Reserved")]
        Reserved = 4
    }

    /// <summary>
    /// Enumeration for logical interface type options
    /// </summary>
    public enum LogicalInterfaceTypeEnum
    {
        /// <summary>
        /// Enum for Loopback
        /// </summary>
        [EnumMember(Value = "Loopback")]
        Loopback = 0,

        /// <summary>
        /// Enum for Tunnel
        /// </summary>
        [EnumMember(Value = "Tunnel")]
        Tunnel = 1
    }

    /// <summary>
    /// Network status enumeration
    /// </summary>
    public enum NetworkStatusEnum
    {
        /// <summary>
        /// Not staged enum
        /// </summary>
        NotStaged = 0,

        /// <summary>
        /// Staged emum
        /// </summary>
        Staged = 1,

        /// <summary>
        /// Active enum
        /// </summary>
        Active = 2,

        /// <summary>
        /// Activation failure enum
        /// </summary>
        ActivationFailure = 3,

        /// <summary>
        /// Staged Inconsisten enum
        /// </summary>
        StagedInconsistent = 4
    }

    /// <summary>
    /// Enumeration of tenant environnent options
    /// </summary>
    /// <value>Enumerated list of tenant environment options</value>
    public enum TenantEnvironmentEnum
    {
        /// <summary>
        /// Enum for Development
        /// </summary>
        Development = 1,

        /// <summary>
        /// Enum for Staging
        /// </summary>
        Staging = 2,

        /// <summary>
        /// Enum for Production
        /// </summary>
        Production = 3
    }
}
