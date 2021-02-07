using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.AuthorizationRequirements;
using Authentication.Controllers;
using Authentication.CustomPolicyProvider;
using Authentication.TransFormaer;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Authorization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Authentication
{
    public class Startup
    {
        // This method gets called by the runtime. Use this method to add services to the container.
        // For more information on how to configure your application, visit https://go.microsoft.com/fwlink/?LinkID=398940
        public void ConfigureServices(IServiceCollection services)
        {
            services.AddAuthentication("CoolieAuth").AddCookie("CoolieAuth",config => {
                config.Cookie.Name = "Grandmas.Cookie";
                config.LoginPath = "/Home/Authenticate";
            });


            services.AddAuthorization(config => {

                //var defaultAuthBuider = new AuthorizationPolicyBuilder();
                //var defaultAuthPolicy = defaultAuthBuider.RequireAuthenticatedUser()
                //.RequireClaim(ClaimTypes.DateOfBirth)
                //.Build();
                //config.DefaultPolicy = defaultAuthPolicy;

                //config.AddPolicy("Claim.DoB", policyBuider => {
                //    policyBuider.RequireClaim(ClaimTypes.DateOfBirth);
                //});

                //config.AddPolicy("Claim.DoB", policyBuider => {
                //    policyBuider.AddRequirements(new CustomRequireClaim(ClaimTypes.DateOfBirth));
                //});


                config.AddPolicy("Addmin", policyBuider =>
                {
                    policyBuider.RequireClaim(ClaimTypes.Role, "Admin");
                });


                config.AddPolicy("Claim.DoB", policyBuider => {
                    policyBuider.RequireCustomClaim(ClaimTypes.DateOfBirth);
                });


            });
            services.AddSingleton<IAuthorizationPolicyProvider, CustomAuthorizationPolicyProvider>();
            services.AddSingleton<IAuthorizationHandler, SecurityLevelHander>();
            services.AddSingleton<IAuthorizationHandler,CustomRequireClaimHander> ();
            services.AddScoped<IAuthorizationHandler, CookieJarAuthorizationHander>();
            services.AddScoped<IClaimsTransformation, ClaimsTransformation>();

            services.AddControllersWithViews(config => {
                var defaultAuthBuider = new AuthorizationPolicyBuilder();
                var defaultAuthPolicy = defaultAuthBuider.RequireAuthenticatedUser()
                .RequireClaim(ClaimTypes.DateOfBirth)
                .Build();
              //  config.Filters.Add(new AuthorizeFilter(defaultAuthPolicy));
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            app.UseAuthentication();
            app.UseRouting();
           
            app.UseAuthorization();
           
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapDefaultControllerRoute();
            });
        }
    }
}
