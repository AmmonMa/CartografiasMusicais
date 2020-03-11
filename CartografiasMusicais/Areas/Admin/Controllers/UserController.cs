using CartografiasMusicais.Business.Context;
using CartografiasMusicais.CrossCutting.ValidationModels.User;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CartografiasMusicais.Areas.Admin.Controllers
{
    [ResponseCache(NoStore = true, Location = ResponseCacheLocation.None)]
    [Area("Admin")]
    [Authorize]
    public class UserController : Controller
    {
        private CoreContext Context;
        private readonly RoleManager<IdentityRole> RoleManager;
        private readonly UserManager<User> UserManager;
        private readonly SignInManager<User> SignInManager;


        public UserController(
             CoreContext context,
             RoleManager<IdentityRole> roleManager,
             UserManager<User> userManager,
             SignInManager<User> signInManager)
        {
            RoleManager = roleManager;
            UserManager = userManager;
            SignInManager = signInManager;
            Context = context;
        }
        public async Task<IActionResult> Index()
        {
            var users = await Context.Users.OrderBy(u => u.Email).ToListAsync();
            return View(users);
        }

        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(RegisterUserViewModel model)
        {
            if (ModelState.IsValid)
            {
                var user = new User
                {
                    UserName = model.Email,
                    Email = model.Email
                };
                var result = await UserManager.CreateAsync(user, model.Password);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                AddErrors(result);
            }
            return View(model);
        }
        public async Task<IActionResult> Edit([FromQuery(Name = "email")] string email)
        {
            var user = await Context.Users.SingleOrDefaultAsync(u => u.Email == email);
            var role = (await UserManager.GetRolesAsync(user)).FirstOrDefault();
            var model = new EditUserViewModel
            {
                Id = user.Id,
                Email = user.Email
            };
            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(EditUserViewModel model)
        {
            var user = await UserManager.FindByIdAsync(model.Id);
            if (ModelState.IsValid)
            {
                user.Email = model.Email;
                user.PasswordHash = UserManager.PasswordHasher.HashPassword(user, model.Password);
                var result = await UserManager.UpdateAsync(user);
                if (result.Succeeded)
                {
                    return RedirectToAction(nameof(Index));
                }
                AddErrors(result);
            }
            return View(model);

        }
        [HttpPost]
        public async Task<IActionResult> Delete(string email)
        {
            var user = await UserManager.FindByEmailAsync(email);
            await UserManager.DeleteAsync(user);
            return Ok();
        }
        private void AddErrors(IdentityResult result)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(string.Empty, error.Description);
            }
        }

    }
}
