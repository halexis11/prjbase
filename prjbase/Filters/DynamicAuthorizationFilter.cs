using prjbase.Models;
using prjbase.Controllers;


using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Controllers;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace prjbase.Filters
{
    public class DynamicAuthorizationFilter : IAsyncAuthorizationFilter
    {
        private readonly prjbaseContext _dbContext;

        public DynamicAuthorizationFilter(prjbaseContext dbContext)
        {
            _dbContext = dbContext;
        }

        public async Task OnAuthorizationAsync(AuthorizationFilterContext context)
        {
            if (!IsProtectedAction(context))
                return;

            if (!IsUserAuthenticated(context))
            {
                context.Result = new UnauthorizedResult();
                return;
            }

            var actionId = GetActionId(context);
            var userName = context.HttpContext.User.Identity.Name;
            try
            {
                var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
                var roles = await (
                    from user in _dbContext.Users
                    join userRole in _dbContext.UserRoles on user.Id equals userRole.UserId
                    join role in _dbContext.Roles on userRole.RoleId equals role.Id
                    where user.UserName == userName
                    select role
                ).ToListAsync();

                foreach (var role in roles)
                {
                    if (role.Access == null)
                        continue;

                    var accessList = JsonConvert.DeserializeObject<IEnumerable<MvcControllerInfo>>(role.Access);
                    if (accessList.SelectMany(c => c.Actions).Any(a => a.Id == actionId))
                        return;
                }
            }
            catch (System.Exception)
            {
                var compiledPageActionDescriptor = (CompiledPageActionDescriptor)context.ActionDescriptor;
                return;
            }
            
            
            

            context.Result = new ForbidResult();
        }

        private bool IsProtectedAction(AuthorizationFilterContext context)
        {
            if (context.Filters.Any(item => item is IAllowAnonymousFilter))
                return false;
            //if (context.ActionDescriptor.)
            try
            {
               //var x = context.ActionDescriptor.ActionConstraints<ActionNameAttribute>;
                var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
                var controllerTypeInfo = controllerActionDescriptor.ControllerTypeInfo;
                var actionMethodInfo = controllerActionDescriptor.MethodInfo;

                var authorizeAttribute = controllerTypeInfo.GetCustomAttribute<AuthorizeAttribute>();
                if (authorizeAttribute != null)
                    return true;

                authorizeAttribute = actionMethodInfo.GetCustomAttribute<AuthorizeAttribute>();
                if (authorizeAttribute != null)
                    return true;
            }
            catch (System.Exception e)
            {

                return true;
            }
            

            return false;
        }

        private bool IsUserAuthenticated(AuthorizationFilterContext context)
        {
            return context.HttpContext.User.Identity.IsAuthenticated;
        }

        private string GetActionId(AuthorizationFilterContext context)
        {
            try
            {
                var controllerActionDescriptor = (ControllerActionDescriptor)context.ActionDescriptor;
                var area = controllerActionDescriptor.ControllerTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;
                var controller = controllerActionDescriptor.ControllerName;
                var action = controllerActionDescriptor.ActionName;

                return $"{area}:{controller}:{action}";
            }
            catch (System.Exception)
            {
                var controllerActionDescriptor = (CompiledPageActionDescriptor)context.ActionDescriptor;
                var area = controllerActionDescriptor.DeclaredModelTypeInfo.GetCustomAttribute<AreaAttribute>()?.RouteValue;
                var controller = controllerActionDescriptor.AreaName;
                var action = controllerActionDescriptor.ModelTypeInfo;

                return controllerActionDescriptor.DisplayName;
                
                
            }
            
        }
    }
}
