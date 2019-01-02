using System;
using System.Security.Claims;

namespace AspNetBase.Common.Utils.Extensions
{
  public static class ClaimsPrincipalExtensions
  {
    public static int GetUserId(this ClaimsPrincipal claimsPrincipal)
    {
      if (claimsPrincipal == null)
        throw new ArgumentNullException(nameof(claimsPrincipal));

      var claim = claimsPrincipal.FindFirst(x => x.Type == ClaimTypes.NameIdentifier);

      if (claim == null)
        throw new InvalidOperationException("Could not find the ID claim for the current claims principal object.");

      int.TryParse(claim.Value, out int userId);
      return userId;
    }
  }
}
