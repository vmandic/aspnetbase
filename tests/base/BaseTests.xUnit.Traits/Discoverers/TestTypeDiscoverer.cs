using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BaseTests.xUnit.Traits
{
  /// <summary>
  /// This class discovers all of the tests and test classes that have
  /// applied the TestType attribute
  /// </summary>
  public class TestTypeDiscoverer : ITraitDiscoverer
  {
    /// <summary>
    /// Gets the trait values from the TestType attribute.
    /// </summary>
    /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
    /// <returns>The trait values.</returns>
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
      var testTypes = traitAttribute.GetConstructorArguments().FirstOrDefault() as string[];

      if (testTypes == null) yield break;

      foreach (var testType in testTypes)
        yield return new KeyValuePair<string, string>(nameof(TestType), testType);
    }
  }
}
