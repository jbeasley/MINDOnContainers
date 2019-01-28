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
    public class BadArgumentsResult : ObjectResult
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="message"></param>
        public BadArgumentsResult(string message)
            : base(new ApiResponse() { Code = "BadArguments", Message = message })
        {
            StatusCode = StatusCodes.Status422UnprocessableEntity;
        }
    }
}
