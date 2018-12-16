using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Core.Models.Identity
{
  public class ExternalLoginDto : IExternalLoginModel
  {
    public ExternalLoginDto(string errorMessage)
    {
      ErrorMessage = errorMessage;
    }
    public ExternalLoginDto(string returnUrl, string loginProvider, ExternalLoginInputModel input)
    {
      ReturnUrl = returnUrl;
      LoginProvider = loginProvider;
      Input = input;
    }

    [BindProperty]
    public ExternalLoginInputModel Input { get; set; }

    public string LoginProvider { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }
  }
}
