namespace MINDOnContainers.Services.Attachment.API.Services.Infrastructure
{
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using System;
    using System.Threading.Tasks;
    using MINDOnContainers.Services.Attachment.Infrastructure;

    public class AttachmentContextSeed
    {
        public Task SeedAsync(AttachmentContext context, IHostingEnvironment env, IOptions<AttachmentSettings> settings, ILogger<AttachmentContextSeed> logger)
        {
            // See https://github.com/dotnet-architecture/eShopOnContainers/blob/dev/src/Services/Ordering/Ordering.API/Infrastructure/OrderingContextSeed.cs for
            // an example of seeding the DB from a csv file
            throw new NotImplementedException();
        }
    }
}
