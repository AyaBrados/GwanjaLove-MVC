using GwanjaLoveProto.Models;
using GwanjaLoveProto.Models.ViewModels;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.Extensions.Primitives;

namespace GwanjaLoveProto.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly SignInManager<AppUser> _signInManager;
        private readonly UserManager<AppUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;

        public AccountController(SignInManager<AppUser> signInManager,
            UserManager<AppUser> userManager,
            RoleManager<IdentityRole> roleManager)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _roleManager = roleManager;
        }
        
        public IActionResult Login(string returnUrl)
        {
            return View(new LoginModel
            {
                Password = "",
                Email = "",
                ReturnUrl = returnUrl,
            });
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginModel loginModel)
        {
            if ((ModelState.IsValid && ModelState["Email"].ValidationState == ModelValidationState.Valid) || (ModelState["Email"].ValidationState == ModelValidationState.Invalid && loginModel.SignInWithUserName && string.IsNullOrEmpty(loginModel.Email)))
            {
                AppUser? user = new AppUser();
                user = loginModel.SignInWithUserName ? await _userManager.FindByNameAsync(loginModel.UserName) : await _userManager.FindByEmailAsync(loginModel.Email);

                if (user != null)
                {
                    var result = await _signInManager.PasswordSignInAsync(user,
                      loginModel.Password, loginModel.RememberMe, false);

                    if (result.Succeeded)
                    {
                        return Redirect(loginModel.ReturnUrl ?? "/Home/Index");
                    }
                }
            }

            ModelState.AddModelError("", "Invalid email or password");
            return View(loginModel);
        }

        [HttpGet]
        public IActionResult Register()
        {
            PopulateRoles();
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterModel registerModel)
        {
            if (ModelState.IsValid)
            {
                var user = new AppUser
                {
                    UserName = registerModel.UserName,
                    Email = registerModel.Email
                };
                
                if (registerModel.AvatarImage != null)
                {
                    using (var memoryStream = new MemoryStream())
                    {
                        await registerModel.AvatarImage.CopyToAsync(memoryStream);
                        user.AvatarImage = memoryStream.ToArray();
                    }
                }

                var result = await _userManager.CreateAsync(user, registerModel.Password);

                if (result.Succeeded)
                {
                    await _userManager.AddToRoleAsync(user, registerModel.Role);
                    return RedirectToAction("Login", "Account", "/Home/Index");
                }
                else
                {
                    foreach(var error in result.Errors)
                    {
                        ModelState.AddModelError("", error.Description);
                    }
                }
            }
            PopulateRoles();
            return View(registerModel);
        }

        [HttpPost]
        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        [AllowAnonymous]
        public IActionResult AccessDenied()
        {
            return View();
        }

        private void PopulateRoles()
        {
            ViewBag.Roles = new SelectList(_roleManager.Roles.Select(x => x.Name).ToList());
        }
    }
}
