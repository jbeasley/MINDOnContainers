using System;
using System.ComponentModel.DataAnnotations;
using System.Reflection;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Mind.Api.Models;
using Mind.Services;
using Mind.Api.Controllers;

namespace Mind.Api.Attributes
{
    /// <summary>
    /// Validates that a tenant exists in the database
    /// </summary>
    public class ValidateTenantExistsAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidateTenantExistsAttribute() : base(typeof(ValidateTenantExistsActionFilter))
        {
        }

        private class ValidateTenantExistsActionFilter : IAsyncActionFilter
        {
            private readonly ITenantService _tenantService;
            public ValidateTenantExistsActionFilter(ITenantService tenantService)
            {
                _tenantService = tenantService;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {             
                var tenantId = context.ActionArguments["tenantId"] as int?;          
                if ((await _tenantService.GetByIDAsync(tenantId.Value, asTrackable: false)) == null)
                {
                    context.ModelState.AddModelError(string.Empty, "Could not find the tenant.");
                    context.Result = new ResourceNotFoundResult(context.ModelState);
                    return;
                }

                await next();
            }
        }
    }
}
