using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using NETCore.MailKit.Core;
namespace IdentityExample.Controllers
{
    public class HomeController : Controller
    {
        private UserManager<IdentityUser> _userManager;
        private SignInManager<IdentityUser> _signInManager;
        private IEmailService _mailService;

        public HomeController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,IEmailService mailService)
        { 
            _userManager = userManager;
            _signInManager = signInManager;
            _mailService = mailService;
        }

        [HttpGet]
        public IActionResult Index()
        {

            var fs = User.Claims;

            return View();
        }

        [Authorize]
        public IActionResult Secret()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string username, string password)
        {
            var user = await _userManager.FindByNameAsync(username);
            if (user != null)
            {
                var signInresult = await _signInManager.PasswordSignInAsync(user, password, false, false);

                if (signInresult.Succeeded)
                {
                    return RedirectToAction("Index");
                }
            }
            return RedirectToAction("Index");
        }
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string username, string password)
        {
            var user = new IdentityUser()
            {
                UserName = username,
                Email = "xuattv@fsoft.com"
            };


            var result = await _userManager.CreateAsync(user, password);

            if (result.Succeeded)
            {
                var code = await _userManager.GenerateEmailConfirmationTokenAsync(user);

                var link = Url.Action(nameof(VerifyEmail),"Home",new {userID = user.Id ,code},Request.Scheme,Request.Host.ToString());
                await _mailService.SendAsync("tet@test.com", "Email verifity",$"<a href=\"{link}\">Verify Email</a>");
                return RedirectToAction("Index");
            }

            return View();
        }
        public async Task<IActionResult> VerifyEmail(string UseriD, string code) 
        {
            var user = await _userManager.FindByIdAsync(UseriD);
            if (user == null)
            {
                return BadRequest();
            }
            var result = await _userManager.ConfirmEmailAsync(user, code);
            if (result.Succeeded)
            {
                return View();
            }
            return BadRequest();
        }

        public IActionResult EmailVerification() => View();


        public async Task<IActionResult> LogOut()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index");
        }

    }
}
