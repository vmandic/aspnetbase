using AspNetBase.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetBase.DataAccess.Entities
{
  public class AppUserToken : IdentityUserToken<int>, IEntityBase<int>
  {
    public int Id { get; set; }
    public Guid Uid { get; set; }
  }
}
