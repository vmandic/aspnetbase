using System;
using Xunit.Sdk;

namespace BaseTests.xUnit.Traits
{
  /// <summary>
  /// Apply this attribute to your target test class with integration test methods.
  /// </summary>
  [TraitDiscoverer(DiscovererTypes.IntegrationTestDiscoverer, Utils.AssemblyName)]
  [AttributeUsage(AttributeTargets.Class)]
  public class IntegrationTestAttribute : Attribute, ITraitAttribute
  {
    public IntegrationTestAttribute(params string[] module) { }
  }
}
