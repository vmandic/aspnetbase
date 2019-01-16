using System;
using AspNetBase.Infrastructure.DataAccess.Base;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;

namespace AspNetBase.Infrastructure.DbInitilizer.Seed.Base
{
  public abstract class SeedBase<TEntity> where TEntity : class, IEntityBase<int>, new()
  {
    public SeedBase(AppDbContext context)
    {
      Context = context ??
        throw new ArgumentNullException(nameof(context));

      SeedSet = Context.Set<TEntity>();
    }

    public DbSet<TEntity> SeedSet { get; }
    public AppDbContext Context { get; }
    public abstract void Run();
  }
}
