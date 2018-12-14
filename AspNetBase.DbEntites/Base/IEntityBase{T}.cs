using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetBase.DataAccess.Base
{
  public interface IEntityBase<TId>
  {
    TId Id { get; set; }
    Guid Uid { get; set; }
  }
}
