using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Core.Models.Identity
{
  public interface IExternalLoginModel
  {
    ExternalLoginInputModel Input { get; set; }

    string LoginProvider { get; set; }

    string ReturnUrl { get; set; }

    string ErrorMessage { get; set; }
  }

  public class ExternalLoginPageModel : PageModel
  {
    [BindProperty]
    public ExternalLoginInputModel Input { get; set; }

    public string LoginProvider { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }
  }
}
