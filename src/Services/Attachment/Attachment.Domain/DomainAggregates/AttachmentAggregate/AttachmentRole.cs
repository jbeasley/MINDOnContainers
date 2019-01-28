using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM.Models
{
    public class AttachmentRole
    {
        public int AttachmentRoleID { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public bool IsTaggedRole { get; set; }
        public bool IsLayer3Role { get; set; }
        public bool RequireContractBandwidth { get; set; }
        public int? RoutingInstanceTypeID { get; set; }
        public bool SupportedByBundle { get; set; }
        public bool SupportedByMultiPort { get; set; }
        public bool RequireSyncToNetwork { get; set; }
        public int PortPoolID { get; set; }
        public virtual PortPool PortPool { get; set; }
        public virtual RoutingInstanceType RoutingInstanceType { get; set; }
        public virtual ICollection<DeviceRoleAttachmentRole> DeviceRoleAttachmentRoles { get; set; }
        public virtual ICollection<VifRole> VifRoles { get; set; }
        [Required]
        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}