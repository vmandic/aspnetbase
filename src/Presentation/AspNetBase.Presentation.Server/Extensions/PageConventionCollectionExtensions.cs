using System;
using System.Linq;
using AspNetBase.Common.Utils.Extensions;
using AspNetBase.Infrastructure.DataAccess.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc.ApplicationModels;
using Microsoft.AspNetCore.Mvc.Authorization;

namespace AspNetBase.Presentation.Server.Extensions
{
  public static class PageConventionCollectionExtensions
  {
    public static PageConventionCollection AuthorizeFolderWithRoles(
      this PageConventionCollection conventions,
      string folderPath,
      params string[] roles)
    {
      if (conventions == null)
        throw new ArgumentNullException(nameof(conventions));

      if (string.IsNullOrEmpty(folderPath))
        throw new ArgumentException("Argument cannot be null or empty.", nameof(folderPath));

      var policy = new AuthorizationPolicyBuilder().RequireRole(roles).Build();
      var authorizeFilter = new AuthorizeFilter(policy);

      conventions.AddPageApplicationModelConvention(
        folderPath,
        model => model.Filters.Add(authorizeFilter));

      return conventions;
    }

    public static PageConventionCollection AuthorizeFolderWithRoles(
        this PageConventionCollection conventions,
        string folderPath,
        params Roles[] roles) =>
      conventions.AuthorizeFolderWithRoles(
        folderPath,
        roles.Select(r => r.ToString().SpaceBetweenCapitals()).ToArray());

    public static PageConventionCollection AuthorizePageWithRoles(
      this PageConventionCollection conventions,
      string pageName,
      string[] roles)
    {
      if (conventions == null)
        throw new ArgumentNullException(nameof(conventions));

      if (string.IsNullOrEmpty(pageName))
        throw new ArgumentException("Argument cannot be null or empty.", nameof(pageName));

      var policy = new AuthorizationPolicyBuilder().RequireRole(roles).Build();
      var authorizeFilter = new AuthorizeFilter(policy);

      conventions.AddPageApplicationModelConvention(
        pageName,
        model => model.Filters.Add(authorizeFilter));

      return conventions;
    }

    public static PageConventionCollection AuthorizePageWithRoles(
        this PageConventionCollection conventions,
        string pageName,
        Roles[] roles) =>
      conventions.AuthorizePageWithRoles(
        pageName,
        roles.Select(r => r.ToString().SpaceBetweenCapitals()).ToArray());
  }
}
