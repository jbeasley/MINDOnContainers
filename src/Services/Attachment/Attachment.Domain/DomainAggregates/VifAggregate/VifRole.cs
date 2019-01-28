using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SCM.Models
{
    public class VifRole
    {
        public int VifRoleID { get; private set; }
        [Required]
        [MaxLength(50)]
        public string Name { get; set; }
        [StringLength(250)]
        public string Description { get; set; }
        public bool IsLayer3Role { get; set; }
        public bool RequireContractBandwidth { get; set; }
        public bool RequireSyncToNetwork { get; set; }
        public int? RoutingInstanceTypeID { get; set; }
        public int AttachmentRoleID { get; set; }
        [Required]
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual AttachmentRole AttachmentRole { get; set; }
        public virtual RoutingInstanceType RoutingInstanceType { get; set; }
        public virtual ICollection<Vif> Vifs { get; set; }
    }
}