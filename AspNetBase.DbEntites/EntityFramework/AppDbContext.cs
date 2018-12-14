using AspNetBase.DataAccess.Base;
using AspNetBase.DataAccess.Entities;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using System;
using System.Linq;

namespace AspNetBase.DataAccess.Data
{
  public class AppDbContext : IdentityDbContext<
    AppUser,
    AppRole,
    int,
    AppUserClaim,
    AppUserRole,
    AppUserLogin,
    AppRoleClaim,
    AppUserToken
  >
  {
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
      base.OnModelCreating(builder);

      // NOTE: commenting code below makes migrations work?
      var entities = typeof(IEntityBase<>)?.Assembly?.GetTypes()?.Where(x => x != null && x.Namespace != null && x.Namespace.Contains("Entities"));

      foreach (var entityType in entities)
      {
        var type = builder.Entity(entityType);
        type.HasKey(nameof(IEntityBase<int>.Id));
        type.Property(nameof(IEntityBase<int>.Uid)).HasDefaultValueSql("NEWID()");
        type.HasIndex(nameof(IEntityBase<int>.Uid)).IsUnique();
        type.ToTable(entityType.Name);
      }
    }
  }
}
