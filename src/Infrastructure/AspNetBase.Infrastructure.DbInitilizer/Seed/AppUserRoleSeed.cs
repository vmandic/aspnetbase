using System;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DbInitilizer.Extensions;
using AspNetBase.Infrastructure.DbInitilizer.Seed.Base;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Infrastructure.DbInitilizer.Seed
{
  [RegisterDependency(ServiceLifetime.Scoped, typeof(AppUserRoleSeed))]
  public class AppUserRoleSeed : SeedBase<AppUserRole>
  {
    public AppUserRoleSeed(AppDbContext context, ILogger<AppUserRole> logger) : base(context, logger) { }

    public override int ExceutionOrder => 3;

    public override void Run()
    {
      Logger.LogInformation("Seeding '{entityName}' data...", nameof(AppUserRole));

      var admintToSysAdminRole = new AppUserRole
      {
        Id = 1,
        RoleId = 1,
        Uid = Guid.Parse("e4c4b20a-ce2b-49d4-afe4-a1f1b2f0cf8a"),
        UserId = 1
      };

      var testToRegUserRole = new AppUserRole
      {
        Id = 2,
        RoleId = 2,
        Uid = Guid.Parse("a9372364-0437-417b-bd71-488dc7b9e0a5"),
        UserId = 2
      };

      Context.AddOrUpdate(admintToSysAdminRole);
      Context.AddOrUpdate(testToRegUserRole);

      Context.SaveChanges();
    }
  }
}
