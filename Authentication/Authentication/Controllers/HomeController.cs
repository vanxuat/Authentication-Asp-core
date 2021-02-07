using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using Authentication.CustomPolicyProvider;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Authentication.Controllers
{
    public class HomeController : Controller
    {
        //public IAuthorizationService _authorizationService;

        //public HomeController(IAuthorizationService authorizationService)
        //{
        //    _authorizationService = authorizationService;
        //}


       
        public IActionResult Index()
        {
            return View();
        }
        [Authorize]
        public IActionResult Secret()
        {
            var cl = User.Claims;
            return View();
        }

        [Authorize(Policy = "Claim.DoB")]
        public IActionResult SecretPolicy()
        {
            var cl = User.Claims;
            return View("Secret");
        }

        [SecurityLevel(5)]
        public IActionResult SercretLevel() 
        {
            return View("Secret");
        }
        [SecurityLevel(10)]
        public IActionResult SecretHightLevel()
        {
            return View("Secret");
        }




        [AllowAnonymous]
        public IActionResult Authenticate()
        {
            var grandmaClaims = new List<Claim>() 
            {
                new Claim(ClaimTypes.Name,"Xuat"),
                new Claim(ClaimTypes.Email,"Xuattv@fsoft.com.vn"),
                new Claim("Grandma.Says","Very good"),
                new Claim(ClaimTypes.DateOfBirth,DateTime.Today.ToString()),
                new Claim(ClaimTypes.Role,"admin"),
                new Claim(DynamicPilicies.SecurityLevel,"7")
            };

            var licenseClaims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name,"Tran"),
                new Claim(ClaimTypes.Email,"Tran@fsoft.com.vn"),
                new Claim("DrivingLiencese","A+"),
                new Claim(ClaimTypes.Role,"user")
            };

            var grandmaIdentity = new  ClaimsIdentity(grandmaClaims, "Grandma Identity");
            var licenseIdentity = new ClaimsIdentity(licenseClaims, "Goverment");
            var userPrincipal = new ClaimsPrincipal(new[] { grandmaIdentity, licenseIdentity });


            HttpContext.SignInAsync(userPrincipal);

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> DoStuff([FromServices] IAuthorizationService _authorizationService)
        {

            var buider = new AuthorizationPolicyBuilder("Schema");

            var customPolicy = buider.RequireClaim("Hello").Build();

            var authResult =  await _authorizationService.AuthorizeAsync(User, customPolicy);

            if (authResult.Succeeded) 
            {
                return View("Index");
            }

            return View("Index");
        }

         
    }
}
