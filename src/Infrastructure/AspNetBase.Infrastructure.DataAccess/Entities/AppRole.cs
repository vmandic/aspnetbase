using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using System;

namespace AspNetBase.Infrastructure.DataAccess.Entities
{
  public class AppRole : IdentityRole<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }
  }
}
