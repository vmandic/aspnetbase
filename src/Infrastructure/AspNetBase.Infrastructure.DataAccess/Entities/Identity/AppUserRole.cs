using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppUserRole : IdentityUserRole<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int Id { get; set; }

    public AppRole Role { get; set; }
    public AppUser User { get; set; }
  }
}
