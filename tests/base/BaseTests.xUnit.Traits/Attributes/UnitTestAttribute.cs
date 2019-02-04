using System;
using Xunit.Sdk;

namespace BaseTests.xUnit.Traits
{
  /// <summary>
  /// Apply this attribute to your target test class with unit tests.
  /// </summary>
  [TraitDiscoverer(DiscovererTypes.TestTypeDiscoverer, Utils.AssemblyName)]
  [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
  public class UnitTestAttribute : Attribute, ITraitAttribute
  {
    public UnitTestAttribute(params string[] module) { }
  }
}
