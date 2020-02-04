using Hangfire.Annotations;
using Hangfire.Dashboard;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace HangFire_Core.Service
{
    public class HangfireAuthorizationFilter : IDashboardAuthorizationFilter
    {
        private string policyName;

        public HangfireAuthorizationFilter(string policyName)
        {
            this.policyName = policyName;
        }

        public bool Authorize([NotNull] DashboardContext context)
        {
            var httpContext = context.GetHttpContext();
            var authService = httpContext.RequestServices.GetRequiredService<IAuthorizationService>();
            var result = authService.AuthorizeAsync(httpContext.User, this.policyName);
            if (!result.Result.Succeeded)
            {
                httpContext.Response.Redirect("login");
            }
            return true;
        }
    }
}
