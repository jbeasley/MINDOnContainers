using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace MINDOnContainers.Services.Attachment.Domain.SeedWork
{
    public interface IModifiableResource
    {
        string ConcurrencyToken { get; }
        byte[] RowVersion { get; set; }
    }
}
