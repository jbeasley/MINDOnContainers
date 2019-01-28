using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Net;

namespace SCM.Models
{
    public static class InterfaceQueryableExtensions
    {
        public static IQueryable<Interface> IncludeDeepProperties(this IQueryable<Interface> query)
        {
            return query.Include(x => x.Ports)
                        .Include(x => x.Vlans)
                        .Include(x => x.Attachment);
        }
    }

    public class Interface { 

        public int InterfaceID { get; private set; }
        public int? DeviceID { get; set; }
        public int AttachmentID { get; set; }
        [MaxLength(15)]
        public string IpAddress { get; set; }
        [MaxLength(15)]
        public string SubnetMask { get; set; }
        [Timestamp]
        public byte[] RowVersion { get; set; }
        public virtual Attachment Attachment { get; set; }
        public virtual Device Device { get; set; }
        public virtual ICollection<Port> Ports { get; set; }
        public virtual ICollection<Vlan> Vlans { get; set; }
    }
}