using System;
using System.Collections.Generic;
using System.Text;
using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Infrastructure.DataAccess.Entities.Identity
{
  public class AppUser : IdentityUser<int>, IEntityBase<int>
  {
    public Guid Uid { get; set; }
    public ICollection<AppUserRole> UserRoles { get; set; } = new HashSet<AppUserRole>();

    internal static void Configure(ModelBuilder mb)
    {
      mb.Entity<AppUser>()
        .HasMany(x => x.UserRoles)
        .WithOne(x => x.User)
        .HasForeignKey(x => x.UserId)
        .IsRequired()
        .OnDelete(DeleteBehavior.Cascade);
    }
  }
}
