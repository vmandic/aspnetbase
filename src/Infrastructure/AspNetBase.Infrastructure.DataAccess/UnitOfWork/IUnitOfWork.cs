using System;
using System.Threading.Tasks;

namespace AspNetBase.Infrastructure.DataAccess.UnitOfWork
{
  public interface IUnitOfWork
  {
    bool Commit();
    Task<bool> CommitAsync();
    event Action AfterCommit;
  }
}
