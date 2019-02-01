using System.Linq;

namespace MINDOnContainers.Services.Attachment.Domain.SeedWork
{
    using System;
    using System.Linq;

    public static class ModifiableResourceExtensions
    {
        public static string GetWeakETag(this IModifiableResource resource) =>
            "\"" + Convert.ToBase64String(resource.RowVersion) + "\"";

        public static byte[]  GetConcurrencyToken(this IModifiableResource resource) =>
            resource.RowVersion;

        public static bool HasBeenModified(this IModifiableResource resource, HttpRequest request)
        {
            var modified = true;

            var requestHeaders = request.GetTypedHeaders();
            if (HttpMethods.IsGet(request.Method) || HttpMethods.IsHead(request.Method))
            {
                var ifNoneMatch = requestHeaders.IfNoneMatch?.Select(v => v.Tag.ToString());

                if ((ifNoneMatch != null) && ifNoneMatch.Any())
                {
                    if (!string.IsNullOrWhiteSpace(resource.ConcurrencyToken)
                        && ifNoneMatch.Contains(resource.ConcurrencyToken))
                    {
                        modified = false;
                    }
                }
            }

            return modified;
        }

        public static bool HasPreconditionFailed(this IModifiableResource resource, HttpRequest request)
        {
            bool preconditionFailed = false;

            var requestHeaders = request.GetTypedHeaders();
            if (HttpMethods.IsPut(request.Method) || HttpMethods.IsPatch(request.Method))
            {
                var ifMatch = requestHeaders.IfMatch?.Select(v => v.Tag.ToString());

                if (ifMatch == null || !ifMatch.Any())
                {
                    preconditionFailed = true;
                }
                else if (!ifMatch.Contains(resource.ConcurrencyToken))
                {
                    preconditionFailed = true;
                }
            }

            return preconditionFailed;
        }

        public static bool HasPreconditionFailed(this IModifiableResource resource, HttpRequest request, string receivedConcurrencyToken)
        {
            bool preconditionFailed = false;

            if (HttpMethods.IsPut(request.Method) || HttpMethods.IsPatch(request.Method) || HttpMethods.IsPost(request.Method))
            {
                if (receivedConcurrencyToken != resource.ConcurrencyToken)
                {
                    preconditionFailed = true;
                }
            }

            return preconditionFailed;
        }

        public static void SetModifiedHttpHeaders(this IModifiableResource resource, HttpResponse response)
        {
            var etag = resource.ConcurrencyToken;
            if (etag != null)
            {
                var responseHeaders = response.GetTypedHeaders();
                responseHeaders.ETag = new EntityTagHeaderValue(etag, true);
            }
        }
    }
}
