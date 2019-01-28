using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using SCM.Data;
using Mind.Api.Controllers;
using Mind.Models;

namespace Mind.Api.Attributes
{
    /// <summary>
    /// Validates that a provider domain attachment exists in the database
    /// </summary>
    public class ValidateProviderDomainAttachmentExistsAttribute : TypeFilterAttribute
    {
        /// <summary>
        /// 
        /// </summary>
        public ValidateProviderDomainAttachmentExistsAttribute() : base(typeof(ValidateProviderDomainAttachmentExistsActionFilter))
        {
        }

        private class ValidateProviderDomainAttachmentExistsActionFilter : IAsyncActionFilter
        {
            private readonly IUnitOfWork _unitOfWork;
            public ValidateProviderDomainAttachmentExistsActionFilter(IUnitOfWork unitOfWork)
            {
                _unitOfWork = unitOfWork;
            }

            public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
            {          
                var attachmentId = context.ActionArguments["attachmentId"] as int?;
               
                if ((from result in await _unitOfWork.AttachmentRepository.GetAsync(q =>
                        q.AttachmentID == attachmentId 
                        && q.AttachmentRole.PortPool.PortRole.PortRoleType == PortRoleTypeEnum.TenantFacing,
                        AsTrackable: false)
                        select result)
                       .SingleOrDefault() == null)
                {
                    context.ModelState.AddModelError(string.Empty, "Could not find the provider domain attachment.");
                    context.Result = new ResourceNotFoundResult(context.ModelState);
                    return;
                }

                await next();
            }
        }
    }
}
