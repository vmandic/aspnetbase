using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Text;
using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppUserRole : IdentityUserRole<int>, IEntityBase<int?>
  {
    public Guid Uid { get; set; }

    [DatabaseGenerated(DatabaseGeneratedOption.Identity)]
    public int? Id { get; set; }

    public AppRole Role { get; set; }
    public AppUser User { get; set; }

    internal static void Configure(ModelBuilder mb)
    {
      var e = mb.Entity<AppUserRole>();
      e.Property(x => x.Id).ValueGeneratedOnAdd();
    }
  }
}
