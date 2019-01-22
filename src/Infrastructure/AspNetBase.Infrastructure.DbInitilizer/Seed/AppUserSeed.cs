using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DbInitilizer.Seed.Base;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Infrastructure.DbInitilizer.Seed
{
  [RegisterDependency(ServiceLifetime.Scoped, typeof(AppUserSeed))]
  internal class AppUserSeed : SeedBase<AppUser>
  {
    const string DEFAULT_PASSWORD = "Abcd1234!";
    private readonly UserManager<AppUser> _userManager;

    public AppUserSeed(AppDbContext context, ILogger<AppUser> logger, UserManager<AppUser> userManager) : base(context, logger) =>
      _userManager = userManager;

    public override int ExecutionOrder => 2;

    public override void Run()
    {
      Logger.LogInformation("Seeding '{entityName}' data...", nameof(AppUser));

      #region Seed data (without passwords)
      var users = new List<AppUser>
      {
        new AppUser
        {
        Id = 1,
        AccessFailedCount = 0,
        Email = "admin@aspnetbase.com",
        ConcurrencyStamp = "c524d4d5-a34e-48ae-91e3-c2aac36524f3",
        SecurityStamp = "c524d4d5-a34e-48ae-91e3-c2aac36524f3",
        PasswordHash = null,
        EmailConfirmed = true,
        LockoutEnabled = false,
        LockoutEnd = null,
        NormalizedEmail = "ADMIN@ASPNETBASE.COM",
        NormalizedUserName = "ADMIN@ASPNETBASE.COM",
        PhoneNumber = null,
        PhoneNumberConfirmed = false,
        TwoFactorEnabled = false,
        Uid = Guid.Parse("190a36b3-1ee7-4c02-bfbe-fab6a612e17f"),
        UserName = "admin@aspnetbase.com"
        },

        new AppUser
        {
        Id = 2,
        AccessFailedCount = 0,
        Email = "test@aspnetbase.com",
        ConcurrencyStamp = "c524d4d5-a34e-48ae-91e3-c2aac36524f3",
        SecurityStamp = "c524d4d5-a34e-48ae-91e3-c2aac36524f3",
        PasswordHash = null,
        EmailConfirmed = true,
        LockoutEnabled = false,
        LockoutEnd = null,
        NormalizedEmail = "TEST@ASPNETBASE.COM",
        NormalizedUserName = "TEST@ASPNETBASE.COM",
        PhoneNumber = null,
        PhoneNumberConfirmed = false,
        TwoFactorEnabled = false,
        Uid = Guid.Parse("b3fcb5fd-f575-44a5-9b6b-c6d5d51c2664"),
        UserName = "test@aspnetbase.com"
        }
      };
      #endregion

      users.ForEach(user =>
      {
        var dbUser = SeedSet.AsNoTracking().SingleOrDefault(x => x.Id == user.Id);

        if (dbUser == null)
        {
          SeedSet.Add(user);
        }
        else
        {
          user.ConcurrencyStamp = dbUser.ConcurrencyStamp;
          SeedSet.Update(user);
        }
      });

      // batch save:
      Context.SaveChanges();

      // update password one by one:
      Task.WhenAll(
          users.Select(user =>
            _userManager.AddPasswordAsync(user, DEFAULT_PASSWORD)))
        .GetAwaiter()
        .GetResult();
    }
  }
}
