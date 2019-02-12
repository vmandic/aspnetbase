using System;
using System.Collections.Generic;
using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppRole : IdentityRole<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = new HashSet<AppUserRole>();

    internal static void Configure(ModelBuilder mb)
    {
      mb.Entity<AppRole>()
        .HasMany(x => x.UserRoles)
        .WithOne(x => x.Role)
        .HasForeignKey(x => x.RoleId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
