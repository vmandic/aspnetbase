using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AspNetBase.Core.Contracts.Services.Identity;
using AspNetBase.Core.Models.Identity;
using AspNetBase.Infrastructure.DataAccess.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Presentation.Server.Areas.Identity.Pages.Account
{
  [AllowAnonymous]
  public class ExternalLoginModel : ExternalLoginPageModel
  {
    private readonly SignInManager<AppUser> _signInManager;
    private readonly UserManager<AppUser> _userManager;
    private readonly ILogger<ExternalLoginModel> _logger;
    private readonly ISignInService _signInService;

    public ExternalLoginModel(
      SignInManager<AppUser> signInManager,
      UserManager<AppUser> userManager,
      ILogger<ExternalLoginModel> logger,
      ISignInService signInService)
    {
      _signInManager = signInManager;
      _userManager = userManager;
      _logger = logger;
      _signInService = signInService;
    }

    public IActionResult OnGetAsync()
    {
      return RedirectToPage("./Login");
    }

    public IActionResult OnPost(string provider, string returnUrl = null)
    {
      return _signInService.ChallengeExternalLoginProvider(provider, returnUrl);
    }

    public async Task<IActionResult> OnGetCallbackAsync(string returnUrl = null, string remoteError = null)
    {
      var (actionResult, externalLoginDto) = await _signInService.LoginWithExternalProvider(returnUrl, remoteError);
      this.MapFromDto(externalLoginDto);

      return actionResult;

      // returnUrl = returnUrl ?? Url.Content("~/");
      // if (remoteError != null)
      // {
      //   ErrorMessage = $"Error from external provider: {remoteError}";
      //   return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
      // }
      // var info = await _signInManager.GetExternalLoginInfoAsync();
      // if (info == null)
      // {
      //   ErrorMessage = "Error loading external login information.";
      //   return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
      // }

      // // Sign in the user with this external login provider if the user already has a login.
      // var result = await _signInManager.ExternalLoginSignInAsync(info.LoginProvider, info.ProviderKey, isPersistent : false, bypassTwoFactor : true);
      // if (result.Succeeded)
      // {
      //   _logger.LogInformation("{Name} logged in with {LoginProvider} provider.", info.Principal.Identity.Name, info.LoginProvider);

      //   return LocalRedirect(returnUrl);
      // }
      // if (result.IsLockedOut)
      // {
      //   return RedirectToPage("./Lockout");
      // }
      // else
      // {
      //   // If the user does not have an account, then ask the user to create an account.
      //   ReturnUrl = returnUrl;
      //   LoginProvider = info.LoginProvider;
      //   if (info.Principal.HasClaim(c => c.Type == ClaimTypes.Email))
      //   {
      //     Input = new ExternalLoginInputModel
      //     {
      //     Email = info.Principal.FindFirstValue(ClaimTypes.Email)
      //     };
      //   }

      //   return Page();
      // }
    }

    public async Task<IActionResult> OnPostConfirmationAsync(string returnUrl = null)
    {
      var (actionResult, externalLoginDto) = await _signInService.ConfirmExternalLogin(ModelState, returnUrl);
      this.MapFromDto(externalLoginDto);

      return actionResult;

      // returnUrl = returnUrl ?? Url.Content("~/");
      // // Get the information about the user from the external login provider
      // var info = await _signInManager.GetExternalLoginInfoAsync();
      // if (info == null)
      // {
      //   ErrorMessage = "Error loading external login information during confirmation.";
      //   return RedirectToPage("./Login", new { ReturnUrl = returnUrl });
      // }

      // if (ModelState.IsValid)
      // {
      //   var user = new AppUser { UserName = Input.Email, Email = Input.Email };
      //   var result = await _userManager.CreateAsync(user);
      //   if (result.Succeeded)
      //   {
      //     result = await _userManager.AddLoginAsync(user, info);
      //     if (result.Succeeded)
      //     {
      //       await _signInManager.SignInAsync(user, isPersistent : false);
      //       _logger.LogInformation("User created an account using {Name} provider.", info.LoginProvider);
      //       return LocalRedirect(returnUrl);
      //     }
      //   }
      //   foreach (var error in result.Errors)
      //   {
      //     ModelState.AddModelError(string.Empty, error.Description);
      //   }
      // }

      // LoginProvider = info.LoginProvider;
      // ReturnUrl = returnUrl;

      // return Page();
    }
  }
}
