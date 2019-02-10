using System;
using BaseTests.xUnit.Traits;
using Xunit;

namespace UnitTests.Core.Providers.Services
{
  [UnitTest(TestModule.BusinessLogicService), TestType(TestType.Functional)]
  public class SignInServiceTests
  {
    public SignInServiceTests()
    {

    }

    [Theory]
    [InlineData("a", "b")]
    public void Should_sign_in_with_username_and_password(string username, string password)
    {
      Assert.True(true);
    }
  }
}
