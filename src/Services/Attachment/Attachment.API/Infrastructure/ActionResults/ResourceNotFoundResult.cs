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
    public class ResourceNotFoundResult : ObjectResult
    {
        /// <summary>
        /// 
        /// </summary>
        public ResourceNotFoundResult(ModelStateDictionary modelState)
            : base(new ApiResponse(modelState)
            {
                Code = "ResourceNotFound",
                Message = "The resource was not found. Check that the arguments you supplied are correct."
            })
        {
            StatusCode = StatusCodes.Status404NotFound;
        }
    }
}
