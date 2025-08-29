using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using HrPortal.EF.Entities;
using HrPortal.Models.Identity;
using HrPortal.Services.Identity;
using HrPortal.Shared.Enums;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace HrPortal.Web.Controllers
{
    public class IdentityController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly PasswordHasher<Account> _hasher = new();
        public IdentityController(IAccountService accountService)
        {
            _accountService = accountService;
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost, AllowAnonymous, ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel vm, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid)
                return View(vm);

            // 1) 以 UserName 取得帳號
            var accResp = await _accountService.FindAccountByUserName(vm.UserName);
            var acc = accResp.Data;

            if (accResp.StatusCode != (long)ReturnCode.Succeeded || acc == null || string.IsNullOrWhiteSpace(acc.PasswordHash))
            {
                ModelState.AddModelError(string.Empty, "帳號或密碼錯誤。");
                return View(vm);
            }

            // 2) 驗證密碼（和你產生 Hash 的方式一致）
            var hasher = new PasswordHasher<Account>();
            var verify = hasher.VerifyHashedPassword(acc, acc.PasswordHash!, vm.Password);
            if (verify == PasswordVerificationResult.Failed)
            {
                ModelState.AddModelError(string.Empty, "帳號或密碼錯誤。");
                return View(vm);
            }

            // 3) 帳號/人員狀態再驗（Enabled / 未刪除 / Person isActive）
            var userResp = await _accountService.VerifyUser(acc.AccountId);
            if (userResp.StatusCode != (long)ReturnCode.Succeeded || userResp.Data == null)
            {
                ModelState.AddModelError(string.Empty, "此帳號無法登入，請聯絡管理員。");
                return View(vm);
            }

            var user = userResp.Data; // 包含 AccountId、UserName、FullName

            // 4) 組 Claims（先不放角色/權限）
            // 最常用且通用的基本 Claims（建議至少這些）：
            // - NameIdentifier：唯一識別（這裡放 AccountId）
            // - Name：顯示名稱（FullName 或 UserName）
            // - 自訂 "username"：你的系統帳號
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, user.AccountId.ToString()),
                new Claim(ClaimTypes.Name, string.IsNullOrWhiteSpace(user.FullName) ? user.UserName ?? vm.UserName : user.FullName),
                new Claim("username", user.UserName ?? vm.UserName)
                // 如需 Email、Phone 之類，可在 VerifyUser 多回傳，或另外查，再加：
                // new Claim(ClaimTypes.Email, user.Email)
                // new Claim(ClaimTypes.MobilePhone, user.Mobile)
            };

            var identity = new ClaimsIdentity(claims, CookieAuthenticationDefaults.AuthenticationScheme);
            var principal = new ClaimsPrincipal(identity);

            // 5) 寫入登入 Cookie
            var props = new AuthenticationProperties
            {
                IsPersistent = vm.RememberMe,
                AllowRefresh = true,
                ExpiresUtc = DateTimeOffset.UtcNow.AddHours(8)
            };

            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal, props);

            // 6) 導回原頁或首頁
            if (!string.IsNullOrWhiteSpace(returnUrl) && Url.IsLocalUrl(returnUrl))
                return Redirect(returnUrl);

            return RedirectToAction("Index", "Home");
        }

        [Authorize]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
            return RedirectToAction(nameof(Login));
        }

        [HttpGet, AllowAnonymous]
        public IActionResult Denied() => View();
    }
}

