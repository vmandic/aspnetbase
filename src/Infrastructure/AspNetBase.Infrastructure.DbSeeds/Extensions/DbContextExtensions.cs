using System;
using System.Collections.Generic;
using System.Linq;
using AspNetBase.Infrastructure.DataAccess.Base;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspNetBase.Infrastructure.DbSeeds.Extensions
{
  public static class DbContextExtensions
  {
    public static IEnumerable<string> FindPrimaryKeyNames<T>(this DbContext dbContext, T entity, IReadOnlyList<IProperty> keyProperties = null)
    {
      return (keyProperties ?? dbContext.FindPrimaryKeyProperties(entity)).Select(p => p.Name);
    }

    public static IEnumerable<object> FindPrimaryKeyValues<T>(this DbContext dbContext, T entity, IReadOnlyList<IProperty> keyProperties = null)
    {
      return (keyProperties ?? dbContext.FindPrimaryKeyProperties(entity)).Select(p => entity.GetPropertyValue(p.Name));
    }

    static IReadOnlyList<IProperty> FindPrimaryKeyProperties<T>(this DbContext dbContext, T entity)
    {
      return dbContext.Model.FindEntityType(typeof(T)).FindPrimaryKey().Properties;
    }

    static object GetPropertyValue<T>(this T entity, string name)
    {
      return entity.GetType().GetProperty(name).GetValue(entity, null);
    }

    public static void AddOrUpdate<TEntity>(this DbContext ctx, TEntity entity) where TEntity : class
    {
      var keyProps = ctx.FindPrimaryKeyProperties(entity);
      var keyVals = ctx.FindPrimaryKeyValues(entity, keyProps).ToArray();
      var dbEntity = ctx.Set<TEntity>().Find(keyVals);

      if (dbEntity != null)
      {
        // NOTE: detach not to cause conflict with the one being updated
        ctx.Entry(dbEntity).State = EntityState.Detached;

        var entry = ctx.Entry(entity);
        entry.State = EntityState.Modified;

        ctx.FindPrimaryKeyNames(entity, keyProps)
          .ForEach(keyPropName => entry.Property(keyPropName).IsModified = false);
      }
      else
      {
        ctx.Add(entity);
      }
    }
  }
}
