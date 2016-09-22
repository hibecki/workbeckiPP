using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
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
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var c = context.RouteData.Values["controller"].ToString();
            var a = context.RouteData.Values["action"].ToString();
            //if ((c != "Home") && (a != "Login") && (a != "RegisterMember"))
            if (c != "Home")
            {
                var memberId = context.HttpContext.Session.GetString("memberId");
                if (memberId == null)
                {
                    context.Result = new RedirectToActionResult("Index", "Home", null);
                }
                else
                {
                    var roleId = context.HttpContext.Session.GetString("roleId");
                    //Check controller name is in list from session!!

                    //throw new UnauthorizedAccessException();
                    //throw new NotImplementedException();
                }
            }
        }
    }
}
