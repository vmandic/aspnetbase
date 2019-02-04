using System;
using Xunit;

namespace UnitTests.Core.Providers.Services
{
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
