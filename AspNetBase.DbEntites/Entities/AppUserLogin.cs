using AspNetBase.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetBase.DataAccess.Entities
{
  public class AppUserLogin : IdentityUserLogin<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }
    public int Id { get; set; }
  }
}
