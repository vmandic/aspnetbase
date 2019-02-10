using System;
using System.Collections.Generic;
using System.Text;

namespace AspNetBase.Common.Utils.Extensions
{
  public static class LazyExtensions
  { }

  public static class Lazy
  {
    public static Lazy<T> Create<T>(Func<T> func)
    {
      if (func == null)
        throw new ArgumentNullException(nameof(func));

      return new Lazy<T>(func);
    }
  }
}
