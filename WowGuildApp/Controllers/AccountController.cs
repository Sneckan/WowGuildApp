using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WowGuildApp.Data;
using WowGuildApp.Models;
using WowGuildApp.RequestObject;

namespace WowGuildApp.Controllers
{
    public class AccountController : Controller
    {

        private ApplicationDbContext db;
        private UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private RoleManager<IdentityRole> roleManager;

        public AccountController(ApplicationDbContext db, UserManager<User> userManager, SignInManager<User> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.db = db;
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Login()
        {
            return View();
        }

        public IActionResult Logout()
        {
            signInManager.SignOutAsync();


            return RedirectToAction("Index", "Home");
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestObject request)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }

            var result = await signInManager.PasswordSignInAsync(request.Email, request.Password, false, false);

            if (!result.Succeeded)
            {
                List<IdentityError> identityErrors = new List<IdentityError>();
                var s = new IdentityError { Code = "1", Description = "Fel lösenord eller användarnamn" };

                identityErrors.Add(s);
                ViewData["Errors"] = identityErrors;
                return View();
            }

            return RedirectToAction("Index", "Event");
        }

        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterRequestObject request)
        {
            
            if (!ModelState.IsValid || request.Password != request.ConfirmPassword)
            {
                return View();
            }
            //User är en ärver från IdentityUser och därför kan vi i följande kodsnutt endast tilldela det två värden.
            User user = new User
            {
                UserName = request.Username,
                Email = request.Email,
            };

            //Här tilldelar vi även lösenordet och det sparas, tillsammanes med User objektet ovan, med hjälp av userManager i Databasen
            IdentityResult result = await userManager.CreateAsync(user, request.Password);

            if (!result.Succeeded)
            {
                ViewData["Errors"] = result.Errors;
                return View();
            }

            //Sist men inte minst, vi vill inte returnera en särskild view bara för detta tillfälle, så vi vidarebefordrar användaren till AccountControll Index.
            return RedirectToAction("Index");
        }
    }
}
