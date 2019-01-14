using System;
using System.Threading.Tasks;
using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Infrastructure.DataAccess.EntityFramework;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace AspNetBase.Infrastructure.DataAccess.UnitOfWork
{
  [RegisterDependency(ServiceLifetime.Scoped)]
  public class UnitOfWork : IUnitOfWork
  {
    private readonly AppDbContext _context;

    public UnitOfWork(AppDbContext context)
    {
      _context = context;
    }

    public event Action AfterCommit;

    public bool Commit()
    {
      try
      {
        var result = _context.SaveChanges() >= 0;

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
        var result = await _context.SaveChangesAsync() >= 0;

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
