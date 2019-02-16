using System.Collections.Generic;

namespace AspNetBase.Core.App.Models.Admin.Roles
{
  public class RoleListVm
  {
    public int Id { get; set; }
    public string Name { get; set; }
    public string UsersForDisplay => string.Join(", ", Users);
    public IEnumerable<string> Users { get; set; }
    public int UserCount { get; set; }
  }
}
