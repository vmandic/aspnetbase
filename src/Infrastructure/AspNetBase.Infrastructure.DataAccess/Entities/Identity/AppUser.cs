using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppUser : IdentityUser<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }
  }
}
