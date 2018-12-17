using Microsoft.Extensions.DependencyInjection;
using System;

namespace AspNetBase.Common.Utils.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class RegisterDependencyAttribute : Attribute
  {
    public RegisterDependencyAttribute(ServiceLifetime lifetime, Type contractType)
    {
      Lifetime = lifetime;
      ContractType = contractType;
    }

    public ServiceLifetime Lifetime { get; }
    public Type ContractType { get; }
  }
}
