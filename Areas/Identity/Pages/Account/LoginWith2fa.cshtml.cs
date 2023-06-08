// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Identity;
using LibBase;

namespace VKR_Poom_Reserving.Areas.Identity.Pages.Account
{
    public class LoginWith2faModel : PageModel
    {
        private readonly SignInManager<User> _signInManager;
        private readonly UserManager<User> _userManager;
        private readonly ILogger<LoginWith2faModel> _logger;

        public LoginWith2faModel(
            SignInManager<User> signInManager,
            UserManager<User> userManager,
            ILogger<LoginWith2faModel> logger)
        {
            _signInManager = signInManager;
            _userManager = userManager;
            _logger = logger;
        }
        [BindProperty]
        public InputModel Input { get; set; }
        public bool RememberMe { get; set; }
        public string ReturnUrl { get; set; }
        public class InputModel
        {
            [Required]
            [StringLength(7, ErrorMessage = "Пароль должен содержать не менее {2} и не более {1} символов.", MinimumLength = 6)]
            [DataType(DataType.Text)]
            [Display(Name = "Код аутентификации")]
            public string TwoFactorCode { get; set; }
            [Display(Name = "Запомнить это устройство")]
            public bool RememberMachine { get; set; }
        }

        public async Task<IActionResult> OnGetAsync(bool rememberMe, string returnUrl = null)
        {
            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();

            if (user == null)
            {
                throw new InvalidOperationException($"Невозможно загрузить двухфакторную аутентификацию для этого пользователя.");
            }

            ReturnUrl = returnUrl;
            RememberMe = rememberMe;

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(bool rememberMe, string returnUrl = null)
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            returnUrl = returnUrl ?? Url.Content("~/");

            var user = await _signInManager.GetTwoFactorAuthenticationUserAsync();
            if (user == null)
            {
                throw new InvalidOperationException($"Невозможно загрузить двухфакторную аутентификацию для этого пользователя.");
            }

            var authenticatorCode = Input.TwoFactorCode.Replace(" ", string.Empty).Replace("-", string.Empty);

            var result = await _signInManager.TwoFactorAuthenticatorSignInAsync(authenticatorCode, rememberMe, Input.RememberMachine);

            var userId = await _userManager.GetUserIdAsync(user);

            if (result.Succeeded)
            {
                _logger.LogInformation("Пользователь с ID '{UserId}' вошёл с 2fa.", user.Id);
                return LocalRedirect(returnUrl);
            }
            else
            {
                _logger.LogWarning("Введён неверный код аутентификации для пользователя с ID '{UserId}'.", user.Id);
                ModelState.AddModelError(string.Empty, "Неверный код аутентификации.");
                return Page();
            }
        }
    }
}
