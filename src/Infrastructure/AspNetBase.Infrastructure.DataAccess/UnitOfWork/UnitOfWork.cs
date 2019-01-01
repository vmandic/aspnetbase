using System;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.DataAccess.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Infrastructure.DataAccess.UnitOfWork
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class UnitOfWork : IUnitOfWork
  {
    private readonly DbContext ctx;

    public UnitOfWork(DbContext ctx)
    {
      this.ctx = ctx;
    }

    public event Action AfterCommit;

    public bool Commit()
    {
      try
      {
        var result = ctx.SaveChanges() > 0;

        if (result)
          AfterCommit?.Invoke();

        return result;
      }
      catch
      {
        throw;
      }
      finally
      {
        AfterCommit = null;
      }
    }

    public async Task<bool> CommitAsync()
    {
      try
      {
        var result = await ctx.SaveChangesAsync() > 0;

        if (result)
          AfterCommit?.Invoke();

        return result;
      }
      catch
      {
        throw;
      }
      finally
      {
        AfterCommit = null;
      }
    }
  }
}
