using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Mind.Models
{
    public interface IModifiableResource
    {
        string ConcurrencyToken { get; }
        byte[] RowVersion { get; set; }
    }
}
