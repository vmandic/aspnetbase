using System;
using System.Collections.Generic;
using System.Linq;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using AspNetBase.Infrastructure.DbInitilizer.Seed.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Infrastructure.DbInitilizer.Seed
{
  [RegisterDependency(ServiceLifetime.Scoped, typeof(AppRoleSeed))]
  internal class AppRoleSeed : SeedBase<AppRole>
  {
    public AppRoleSeed(AppDbContext context, ILogger<AppRole> logger) : base(context, logger) { }

    public override int ExecutionOrder => 1;

    public override void Run()
    {
      Logger.LogInformation("Seeding '{entityName}' data...", nameof(AppRole));

      var entities = new List<AppRole>
      {
        new AppRole
        {
        Id = 1,
        Name = "System Administrator",
        ConcurrencyStamp = "c211894e-6247-4d0e-92a8-5a05cd2b58d9",
        Uid = Guid.Parse("d8768ba2-a233-412e-8a7c-227cb4816858")
        },

        new AppRole
        {
        Id = 2,
        Name = "Regular User",
        ConcurrencyStamp = "877a5033-b5b0-4c70-af3f-74cf7b5fe433",
        Uid = Guid.Parse("e1a82e15-b603-47d9-b863-796d903f8db8")
        }
      };

      entities.ForEach(e =>
      {
        var dbEntity = SeedSet.AsNoTracking().SingleOrDefault(x => x.Id == e.Id);

        if (dbEntity == null)
        {
          SeedSet.Add(e);
        }
        else
        {
          e.ConcurrencyStamp = dbEntity.ConcurrencyStamp;
          SeedSet.Update(e);
        }
      });

      Context.SaveChanges();
    }
  }
}
