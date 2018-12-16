using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Core.Models.Identity
{
  public class ExternalLoginDto : IExternalLoginModel
  {
    [BindProperty]
    public ExternalLoginInputModel Input { get; set; }

    public string LoginProvider { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }
  }
}
