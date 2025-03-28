﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc;

namespace MvcOAuthEmpleados.Filters
{
    public class AuthorizeEmpleadosAttribute : AuthorizeAttribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (user.Identity.IsAuthenticated == false)
            {
                RouteValueDictionary routeLogin = new RouteValueDictionary(new
                    {
                        controller = "Managed",
                        action = "Login"
                    });
                context.Result = new RedirectToRouteResult(routeLogin);
            }
        }
    }
}
