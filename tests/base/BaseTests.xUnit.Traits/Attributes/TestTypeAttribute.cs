using System;
using Xunit.Sdk;

namespace BaseTests.xUnit.Traits
{
  /// <summary>
  /// Apply this attribute to your test target to specify a test type.
  /// </summary>
  [TraitDiscoverer(DiscovererTypes.TestTypeDiscoverer, Utils.AssemblyName)]
  [AttributeUsage(AttributeTargets.Method | AttributeTargets.Class, AllowMultiple = true)]
  public class TestTypeAttribute : Attribute, ITraitAttribute
  {
    public TestTypeAttribute(params string[] testType) { }
  }
}
