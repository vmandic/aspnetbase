using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppRoleClaim : IdentityRoleClaim<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }
  }
}
