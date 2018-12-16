using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetBase.Infrastructure.DataAccess.Base
{
  public interface IEntityBase<TId>
  {
    TId Id { get; set; }
    Guid Uid { get; set; }
  }
}
