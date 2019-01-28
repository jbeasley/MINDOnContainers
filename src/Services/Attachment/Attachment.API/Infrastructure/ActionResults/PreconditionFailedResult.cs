using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Mind.Api.Models;

namespace Mind.Api.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class PreconditionFailedResult : ObjectResult
    {
        /// <summary>
        /// 
        /// </summary>
        public PreconditionFailedResult()
            : base(new ApiResponse()
            {
                Code = "PreconditionFailed",
                Message = "The resource has been modified since the 'If-Match' header in your HTTP request was last updated. " +
                "Refresh the value of your 'If-Match' header by fetching the resource and then try the update again."
            })
        {
            StatusCode = StatusCodes.Status412PreconditionFailed;
        }
    }
}
