namespace Weblog.Web
{
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.AspNetCore.Mvc.Filters;
    using System;
    using System.Linq;
    using System.Security.Claims;

    public class AdminAuthorizeAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity!.IsAuthenticated)
            {
                var userIdClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.NameIdentifier)?.Value;
                var usernameClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Name)?.Value;

                if (userIdClaim != "1" || usernameClaim.ToLower() != "admin")
                {
                    context.Result = new ForbidResult();
                }
            }
            else
            {
                context.Result = new ChallengeResult();
            }
        }
    }

}
