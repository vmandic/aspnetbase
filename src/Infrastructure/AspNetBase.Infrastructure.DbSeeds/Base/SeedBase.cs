using System;
using AspNetBase.Infrastructure.DataAccess.Base;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AspNetBase.Infrastructure.DbSeeds.Base
{
  public abstract class BaseSeedBase<TEntity> : ISeed where TEntity : class
  {
    public BaseSeedBase(AppDbContext context, ILogger<TEntity> logger)
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

  public abstract class SeedBase<TEntity, TKey> : BaseSeedBase<TEntity>, ISeed where TEntity : class, IEntityBase<TKey>, new()
  {
    public SeedBase(AppDbContext context, ILogger<TEntity> logger) : base(context, logger) { }
  }

  public abstract class SeedBase<TEntity> : BaseSeedBase<TEntity>, ISeed where TEntity : class, IEntityBase<int>, new()
  {
    public SeedBase(AppDbContext context, ILogger<TEntity> logger) : base(context, logger) { }
  }
}
