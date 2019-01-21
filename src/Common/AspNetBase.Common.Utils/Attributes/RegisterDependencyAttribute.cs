using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetBase.Common.Utils.Attributes
{
  [Flags]
  public enum ServiceInjectionStyle
  {
    Instant = 0,
    Func = 1,
    Lazy = 2,
    All = Instant | Func | Lazy
  }

  [AttributeUsage(AttributeTargets.Class)]
  public class RegisterDependencyAttribute : Attribute
  {
    public RegisterDependencyAttribute(
      ServiceLifetime lifetime,
      Type contractType = null,
      ServiceInjectionStyle injectionStyle = ServiceInjectionStyle.Instant)
    {
      Lifetime = lifetime;
      ContractType = contractType;
      InjectionStyle = injectionStyle;
    }

    public ServiceLifetime Lifetime { get; }
    public Type ContractType { get; }
    public ServiceInjectionStyle InjectionStyle { get; }
  }
}
