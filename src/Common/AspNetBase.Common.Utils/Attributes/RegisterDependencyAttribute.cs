using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetBase.Common.Utils.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class RegisterDependencyAttribute : Attribute
  {
    public RegisterDependencyAttribute(ServiceLifetime lifetime)
    {
      Lifetime = lifetime;
    }

    public ServiceLifetime Lifetime { get; }
  }
}
