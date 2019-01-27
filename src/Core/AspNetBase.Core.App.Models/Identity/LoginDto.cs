using System;

namespace AspNetBase.Core.App.Models.Identity
{
  public class LoginDto
  {
    public LoginDto(string email, string password, bool rememberMe, bool lockoutOnFailure = true)
    {
      if (string.IsNullOrWhiteSpace(email))
        throw new ArgumentNullException(nameof(email));

      if (string.IsNullOrWhiteSpace(password))
        throw new ArgumentNullException(nameof(password));

      Email = email;
      Password = password;
      RememberMe = rememberMe;
      LockoutOnFailure = lockoutOnFailure;
    }

    public string Email { get; set; }
    public string Password { get; set; }
    public bool RememberMe { get; set; }
    public bool LockoutOnFailure { get; set; } = true;
  }
}
