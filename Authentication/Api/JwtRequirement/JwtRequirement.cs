﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace Api.AuthJwtRequirement
{
    public class JwtRequirement : IAuthorizationRequirement { }

    public class JwtRequirementHandler : AuthorizationHandler<JwtRequirement>
    {
        private readonly HttpClient _client;
        private readonly HttpContext _httpcontext;

        public JwtRequirementHandler(IHttpClientFactory httpClientFactory,
            IHttpContextAccessor  httpContextAccessor
            )
        {
            _client = httpClientFactory.CreateClient();
            _httpcontext = httpContextAccessor.HttpContext;
        }

        protected override async Task HandleRequirementAsync(
            AuthorizationHandlerContext context, 
            JwtRequirement requirement)
        {
             if (_httpcontext.Request.Headers.TryGetValue("Authorization", out var authHeader))
            {
                var accessToken = authHeader.ToString().Split(' ')[1];
                var Response = await _client.GetAsync($"https://localhost:44375/Oauth/Validate?access_token={accessToken}");

                if (Response.StatusCode == HttpStatusCode.OK)
                {
                    context.Succeed(requirement);
                }
            }

            throw new NotImplementedException();
        }
    }
}
