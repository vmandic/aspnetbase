using System;
using System.Collections.Generic;
using System.Linq;
using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspNetBase.Infrastructure.DbInitilizer.Extensions
{
  public static class DbContextExtensions
  {
    public static IEnumerable<string> FindPrimaryKeyNames<T>(this DbContext dbContext, T entity)
    {
      return dbContext.FindPrimaryKeyProperties(entity).Select(p => p.Name);
    }

    public static IEnumerable<object> FindPrimaryKeyValues<T>(this DbContext dbContext, T entity)
    {
      return dbContext.FindPrimaryKeyProperties(entity).Select(p => entity.GetPropertyValue(p.Name));
    }

    static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this DbContext dbContext, T entity)
    {
      return dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
    }

    static object GetPropertyValue<T>(this T entity, string name)
    {
      return entity.GetType().GetProperty(name).GetValue(entity, null);
    }

    public static void AddOrUpdate<TEntity>(this DbContext ctx, TEntity entity) where TEntity : class, IEntityBase<int>
    {
      var existsInDb = ctx.Set<TEntity>().Any(x => x.Id == entity.Id);

      if (existsInDb)
      {
        var entry = ctx.Entry(entity);
        entry.State = EntityState.Modified;
        entry.Property(x => x.Id).IsModified = false;
      }
      else
      {
        ctx.Add(entity);
      }
    }
  }
}
