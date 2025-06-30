using Microsoft.AspNetCore.Authorization;
using System.Linq;
using System.Threading.Tasks;
namespace StockApp.Infra.Data.Identity.Authorization
{
    public class PermissionHandler : AuthorizationHandler<PermissionRequirement>
    {
        protected override Task HandleRequirementAsync(AuthorizationHandlerContext context, PermissionRequirement requirement)
        {
            if (context.User.Claims.Any(c => c.Type == "Permission" && c.Value == requirement.Permission))
            {
                context.Succeed(requirement);
            }
            else
            {
                context.Fail();
            }
            return Task.CompletedTask;
        }
    }
}
