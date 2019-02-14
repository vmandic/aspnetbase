using System.Collections.Generic;

namespace AspNetBase.Core.App.Models.Admin.Users
{
  public class UserListVm
  {
    public int Id { get; set; }
    public string Email { get; set; }
    public bool EmailConfirmed { get; set; }
    public IEnumerable<string> Roles { get; set; }
    public string RolesForDisplay => string.Join(", ", Roles);
    public bool TwoFaEnabled { get; set; }
  }
}
