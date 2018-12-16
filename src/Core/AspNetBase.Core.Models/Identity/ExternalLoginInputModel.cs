using System.ComponentModel.DataAnnotations;

namespace AspNetBase.Core.Models.Identity
{
  public class ExternalLoginInputModel
  {

    [Required]
    [EmailAddress]
    public string Email { get; set; }

  }
}
