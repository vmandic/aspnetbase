using System.Collections.Generic;
using System.Linq;
using Xunit.Abstractions;
using Xunit.Sdk;

namespace BaseTests.xUnit.Traits
{
  /// <summary>
  /// This class discovers all of the tests and test classes that have
  /// applied the UnitTest attribute
  /// </summary>
  public class UnitTestDiscoverer : ITraitDiscoverer
  {
    /// <summary>
    /// Gets the trait values from the UnitTest attribute.
    /// </summary>
    /// <param name="traitAttribute">The trait attribute containing the trait values.</param>
    /// <returns>The trait values.</returns>
    public IEnumerable<KeyValuePair<string, string>> GetTraits(IAttributeInfo traitAttribute)
    {
      yield return new KeyValuePair<string, string>(nameof(TestLevel), TestLevel.Unit);

      var testedModules = traitAttribute.GetConstructorArguments().FirstOrDefault() as string[];

      if (testedModules == null) yield break;

      foreach (var module in testedModules)
        yield return new KeyValuePair<string, string>(nameof(TestModule), module);
    }
  }
}
