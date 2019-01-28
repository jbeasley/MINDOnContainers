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
    public class ValidationFailedResult : ObjectResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="modelState"></param>
        public ValidationFailedResult(ModelStateDictionary modelState)
            : base(new ApiResponse(modelState) { Code = "ValidationFailed" })
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public ValidationFailedResult(string message)
            : base(new ApiResponse() { Code = "ValidationFailed", Message = message })
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
