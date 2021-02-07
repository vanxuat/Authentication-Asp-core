﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Authorization.Infrastructure;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Authentication.Controllers
{
    public class OperationsController : Controller
    {
        private IAuthorizationService _authorizationService;
        public OperationsController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        public async Task<IActionResult> Open() 
        {
            var cookieJar = new CookieJar();

            //var requirement = new OperationAuthorizationRequirement()
            //{
            //    Name = CookieJarOperations.ComeNear
            //};
            await _authorizationService.AuthorizeAsync(User, cookieJar, CookieJarAuthOperations.Open);
            return View();

        }

    }


    public class CookieJarAuthorizationHander : AuthorizationHandler<OperationAuthorizationRequirement,CookieJar>
    {
        protected override Task HandleRequirementAsync(
            AuthorizationHandlerContext context,
            OperationAuthorizationRequirement requirement,
            CookieJar cookieJar)
        {
            if (requirement.Name == CookieJarOperations.Look)
            {
                if (context.User.Identity.IsAuthenticated)
                {
                    context.Succeed(requirement);
                }
            }
            else if (requirement.Name == CookieJarOperations.ComeNear)
            {
                if (context.User.HasClaim("Friend", "Good")) 
                {
                    context.Succeed(requirement);
                }
            }
            return Task.CompletedTask;
        }
    }


    public static class CookieJarAuthOperations 
    {
        public static OperationAuthorizationRequirement Open = new OperationAuthorizationRequirement()
        {
            Name = CookieJarOperations.open
        };
    }


    public static class CookieJarOperations 
    {
        public static string open = "Open";
        public static string TakeCookie = "TakeCookie";
        public static string ComeNear = "ComeNear";
        public static string Look = "Look";
    }

    public class CookieJar
    {
        public string Name { get; set; }
    }


}
