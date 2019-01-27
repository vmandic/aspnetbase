namespace AspNetBase.Core.App.Models.Identity
{
  public interface IExternalLoginModel
  {
    ExternalLoginInputModel Input { get; set; }

    string LoginProvider { get; set; }

    string ReturnUrl { get; set; }

    string ErrorMessage { get; set; }
  }
}
