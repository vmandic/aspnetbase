using AspNetBase.Common.Utils.Attributes;
using AspNetBase.Core.Composition.Extensions;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;

namespace AspNetBase.Core.Composition
{
  public static class CompositionRoot
  {
    public static IServiceProvider Initialize(IServiceCollection services)
    {
      services.RegisterExportedTypes();

      return services.BuildServiceProvider();
    }

   
  }
}
