using System;
using AspNetBase.Infrastructure.DataAccess.Base;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Infrastructure.DbInitilizer.Seed.Base
{
  public abstract class SeedBase<TEntity> : ISeed where TEntity : class, IEntityBase<int>, new()
  {
    public SeedBase(AppDbContext context, ILogger<TEntity> logger)
    {
      Context = context ??
        throw new ArgumentNullException(nameof(context));

      Logger = logger ??
        throw new ArgumentNullException(nameof(logger));

      SeedSet = Context.Set<TEntity>();
    }

    public DbSet<TEntity> SeedSet { get; }
    public AppDbContext Context { get; }
    public ILogger<TEntity> Logger { get; }
    public virtual bool Skip => false;
    public abstract int ExecutionOrder { get; }
    public abstract void Run();
  }
}
