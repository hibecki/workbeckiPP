using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace PPcore.Filters
{
    public class AuthorizationPPFilter : IAuthorizationFilter
    {
        //public AuthorizationPPFilter(AuthorizationPolicy policy): base(policy)
        //{
        //}

        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var c = context.RouteData.Values["controller"].ToString();
            var a = context.RouteData.Values["action"].ToString();
            if (c == "Home" && a == "")
            {
                throw new UnauthorizedAccessException();
            }
            else
            {
                //throw new NotImplementedException();
            }
        }

        //public override Task OnAuthorizationAsync(AuthorizationFilterContext context)
        //{
        //    //context.Controller.ViewBag.AutherizationMessage = "Custom Authorization: Message from OnAuthorization method.";
        //    // If there is another authorize filter, do nothing
        //    if (context.Filters.Any(item => item is IAsyncAuthorizationFilter && item != this))
        //    {
        //        return Task.FromResult(0);
        //    }

        //    //Otherwise apply this policy
        //    return base.OnAuthorizationAsync(context);
        //}

        //Entities context = new Entities(); // my entity  
        //    private readonly string[] allowedroles;
        //    public CustomAuthorizeAttribute(params string[] roles)
        //    {
        //        this.allowedroles = roles;
        //    }
        //    protected override bool AuthorizeCore(HttpContextBase httpContext)
        //    {
        //        bool authorize = false;
        //        foreach (var role in allowedroles)
        //        {
        //            var user = context.AppUser.Where(m => m.UserID == GetUser.CurrentUser/* getting user form current context */ && m.Role == role &&
        //            m.IsActive == true); // checking active users with allowed roles.  
        //            if (user.Count() > 0)
        //            {
        //                authorize = true; /* return true if Entity has current user(active) with specific role */
        //            }
        //        }
        //        return authorize;
        //    }
        //    protected override void HandleUnauthorizedRequest(AuthorizationContext filterContext)
        //    {
        //        filterContext.Result = new HttpUnauthorizedResult();
        //    }



    }
}
