using System;
using System.Linq;
using AspNetBase.Infrastructure.DataAccess.Base;
using AspNetBase.Infrastructure.DataAccess.Entities.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AspNetBase.Infrastructure.DataAccess.EntityFramework
{
  public class AppDbContext : IdentityDbContext<AppUser, AppRole, int, AppUserClaim, AppUserRole, AppUserLogin, AppRoleClaim, AppUserToken>
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      ConfigureAllEntities(builder);
    }

    private static void ConfigureAllEntities(ModelBuilder builder)
    {
      var entities = typeof(IEntityBase<>)?.Assembly ?
        .GetTypes() ?
        .Where(x =>
          x != null &&
          x.IsClass &&
          x.Namespace != null &&
          x.Namespace.Contains("Entities")) ?
        .Distinct();

      foreach (var entityType in entities)
      {
        var type = builder.Entity(entityType);

        // NOTE: careful not to override the Identity defaults due to the default UserStore impl. relying on it
        if (!entityType.Namespace.EndsWith("Identity"))
        {
          type.HasKey(nameof(IEntityBase<int>.Id));
        }

        type.Property(nameof(IEntityBase<int>.Uid)).HasValueGenerator<GuidValueGenerator>();
        type.HasIndex(nameof(IEntityBase<int>.Uid)).IsUnique();
        type.ToTable(entityType.Name);
      }
    }
  }
}
