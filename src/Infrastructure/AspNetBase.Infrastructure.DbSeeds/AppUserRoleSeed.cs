using System;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DataAccess.Enums;
using AspNetBase.Infrastructure.DbSeeds.Base;
using AspNetBase.Infrastructure.DbSeeds.Extensions;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Infrastructure.DbSeeds
{
  [RegisterDependency(ServiceLifetime.Scoped, typeof(AppUserRoleSeed))]
  public class AppUserRoleSeed : SeedBase<AppUserRole, int?>
  {
    public AppUserRoleSeed(AppDbContext context, ILogger<AppUserRole> logger) : base(context, logger) { }

    public override int ExecutionOrder => 3;

    public override void Run()
    {
      Logger.LogInformation("Seeding '{entityName}' data...", nameof(AppUserRole));

      var adminToSysAdminRole = new AppUserRole
      {
        Id = 1,
        RoleId = (int) Roles.SystemAdministrator,
        Uid = Guid.Parse("e4c4b20a-ce2b-49d4-afe4-a1f1b2f0cf8a"),
        UserId = 1
      };

      var testToRegUserRole = new AppUserRole
      {
        Id = 2,
        RoleId = (int) Roles.RegularUser,
        Uid = Guid.Parse("a9372364-0437-417b-bd71-488dc7b9e0a5"),
        UserId = 2
      };

      var adminToRegUserRole = new AppUserRole
      {
        Id = 3,
        RoleId = (int) Roles.RegularUser,
        Uid = Guid.Parse("2448dd6b-5711-4409-8db4-602f6bdd1e77"),
        UserId = 1
      };

      Context.AddOrUpdate(adminToSysAdminRole);
      Context.AddOrUpdate(adminToRegUserRole);
      Context.AddOrUpdate(testToRegUserRole);

      Context.SaveChanges();
    }
  }
}
