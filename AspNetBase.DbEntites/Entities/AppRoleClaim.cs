using AspNetBase.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetBase.DataAccess.Entities
{
  public class AppRoleClaim : IdentityRoleClaim<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }
  }
}
