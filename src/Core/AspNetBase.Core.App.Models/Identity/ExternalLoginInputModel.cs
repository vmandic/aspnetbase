using System.ComponentModel.DataAnnotations;

namespace AspNetBase.Core.App.Models.Identity
{
  public class ExternalLoginInputModel
  {

    [Required]
    [EmailAddress]
    public string Email { get; set; }

  }
}
