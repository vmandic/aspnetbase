using System;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using Microsoft.EntityFrameworkCore.ValueGeneration;

namespace AspNetBase.DataAccess.Utilities
{
  public class GuidValueGenerator : ValueGenerator<Guid>
  {
    public override bool GeneratesTemporaryValues => false;

    public override Guid Next(EntityEntry entry) => Guid.NewGuid();
  }
}
