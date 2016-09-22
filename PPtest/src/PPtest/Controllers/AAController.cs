using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using PPtest.Services;
using Microsoft.AspNetCore.Authorization;
using PPtest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using PPtest.Data;

namespace PPtest.Controllers
{

    //Not finish but it work if revised




    //[AllowAnonymous]
    //[Authorize]
    public class AAController : Controller
    {
        private readonly ApplicationDbContext _appcontext;
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger _logger;

        public AAController(ApplicationDbContext appcontext, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ILoggerFactory loggerFactory)
        {
            _appcontext = appcontext; ;
            _userManager = userManager;
            _signInManager = signInManager;
            _logger = loggerFactory.CreateLogger<AAController>();
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult ControlledPage()
        {
            return View();
        }

        public IActionResult Error()
        {
            return View();
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string uname, string upwd, string ReturnUrl)
        {
            ApplicationUser au = new ApplicationUser();
            au.UserName = uname;

            var result = await _signInManager.PasswordSignInAsync(uname, upwd, false, lockoutOnFailure: false);
            if (result.Succeeded)
            {
                _logger.LogInformation(1, "User logged in.");
                //return RedirectToAction(nameof(HomeController.Index), "Home");
                return RedirectToAction(nameof(AAController.ControlledPage), "AA");
                //if (!string.IsNullOrEmpty(ReturnUrl) && Url.IsLocalUrl(ReturnUrl))
                //{
                //    //Re-check this if in case user try to get on page with correct authentication but wrong authorization
                //    return Redirect(ReturnUrl);

                //}
                //else
                //{
                //    return RedirectToAction(nameof(HomeController.Index), "Home");
                //    //return Json(new { result = "success" });
                //}

            }
            else
            {
                return RedirectToAction(nameof(AAController.Error), "AA");
            }
        }

        [HttpGet]
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Create(string email, string fname, string lname)
        {
            var user = new ApplicationUser { UserName = "test1", Email = email };
            var result = await _userManager.CreateAsync(user, "password");

            if (result.Succeeded)
            {
                await _userManager.AddToRoleAsync(user, "Members");
                System.Security.Claims.Claim cl = new System.Security.Claims.Claim("fullName", fname + " " + lname);
                await _userManager.AddClaimAsync(user, cl);
                //await _signInManager.SignInAsync(user, isPersistent: false);
                _logger.LogInformation(3, "User created a new account with password.");
                //SendEmail(email, cid_card, password);
            }
            else
            {
                return Json(new { result = "fail", error_code = -1, error_message = "userManaget cannot create user!" });
            }
            return Json(new { result = "success" });
        }

        [HttpGet]
        public async Task<IActionResult> CreateRole(string roleName)
        {
            var roleManager = new RoleManager<IdentityRole>(new RoleStore<IdentityRole>(_appcontext), null, null, null, null, null);
            if (!await roleManager.RoleExistsAsync("Administrators"))
            {
                await roleManager.CreateAsync(new IdentityRole("Administrators"));
            }
            if (!await roleManager.RoleExistsAsync("Operators"))
            {
                await roleManager.CreateAsync(new IdentityRole("Operators"));
            }
            if (!await roleManager.RoleExistsAsync("Members"))
            {
                await roleManager.CreateAsync(new IdentityRole("Members"));
            }
            if (!await roleManager.RoleExistsAsync(roleName))
            {
                await roleManager.CreateAsync(new IdentityRole(roleName));
            }
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            //var user = new ApplicationUser { UserName = "admin", Email = "info@palangpanya.com" };
            //var result = await _userManager.CreateAsync(user, "admin1");
            //if (result.Succeeded)
            //{
            //    await _userManager.AddToRoleAsync(user, "Administrators");
            //}

            await _signInManager.SignOutAsync();
            _logger.LogInformation(4, "User logged out.");
            return RedirectToAction(nameof(HomeController.Index), "Home");
        }


    }
}