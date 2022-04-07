using Core.Security;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using TKMS.Abstraction.ComplexModels;
using TKMS.Abstraction.Enums;
using TKMS.Abstraction.Models;
using TKMS.Service.Interfaces;

namespace TKMS.Web.Controllers
{
    public class AccountController : BaseController
    {
        private readonly IUserService _userService;

        public AccountController(
           IUserService userService
        )
        {
            _userService = userService;
        }

        [AllowAnonymous]
        public IActionResult Index(string returnUrl = "")
        {
            return View(new LoginModel { ReturnUrl = returnUrl });
        }

        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> Index(LoginModel loginModel)
        {
            if (!ModelState.IsValid) return View(loginModel);

            if (string.IsNullOrEmpty(loginModel.StaffId) || string.IsNullOrEmpty(loginModel.Password))
            {
                SetNotification("Please enter staff id/password.", NotificationTypes.Error, "Login");
                ModelState.AddModelError("", "Please enter staff id/password.");
                return View(loginModel);
            }

            var userResult = await _userService.AuthenticateUser(loginModel);

            if (userResult.Success)
            {
                var user = userResult.Data.GetType().GetProperty("User").GetValue(userResult.Data, null) as User;

                var claims = new[]
                {
                    new Claim(ClaimTypes.Name, user.FullName),
                    new Claim("UserId", user.UserId.ToString()),
                    new Claim("BranchId", user.BranchId.ToString()),
                    new Claim("BranchCode", user.BranchCode),
                    new Claim("RoleTypeId", user.RoleTypeId.ToString()),
                    new Claim("RegionId", user.Branch != null ? user.Branch.RegionId.ToString() : "0"),
                    new Claim("RoleIds", string.Join(",", user.UserRoles.Select(r => r.RoleId))),
                };

                var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);

                await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, new ClaimsPrincipal(identity), new AuthenticationProperties
                {
                    IsPersistent = false
                }); ;

                if (!string.IsNullOrEmpty(loginModel.ReturnUrl) && Url.IsLocalUrl(loginModel.ReturnUrl))
                {
                    return Redirect(loginModel.ReturnUrl);
                }
                else
                {
                    return RedirectToAction("Index", "Home");
                }
            }

            SetNotification(userResult.Message, NotificationTypes.Error, "Login");
            ModelState.AddModelError("", userResult.Message);
            return View(loginModel);
        }

        [HttpGet]
        [Route("Logout")]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            foreach (var cookie in HttpContext.Request.Cookies)
            {
                Response.Cookies.Delete(cookie.Key);
            }
            return RedirectToAction("Index", "Account");
        }
    }
}
