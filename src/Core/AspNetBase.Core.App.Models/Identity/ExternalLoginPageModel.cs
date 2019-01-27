using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AspNetBase.Core.App.Models.Identity
{
  public class ExternalLoginPageModel : PageModel, IExternalLoginModel
  {
    [BindProperty]
    public ExternalLoginInputModel Input { get; set; }

    public string LoginProvider { get; set; }

    public string ReturnUrl { get; set; }

    [TempData]
    public string ErrorMessage { get; set; }

    public void MapFromDto(IExternalLoginModel dto)
    {
      if (dto != null)
      {
        this.LoginProvider = dto.LoginProvider;
        this.ReturnUrl = dto.ReturnUrl;
        this.ErrorMessage = dto.ErrorMessage;

        if (dto.Input != null)
        {
          this.Input = dto.Input;
        }
        
      }
    }
  }
}
