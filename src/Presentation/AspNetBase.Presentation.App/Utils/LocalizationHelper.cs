using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using AspNetBase.Core.Settings;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Presentation.App.Utils
{
  internal class LocalizationHelper
  {
    internal static void ConfigureLocalizationCookieProvider(
      LocalizationSettings localizationSettings,
      RequestLocalizationOptions localiztionOptions)
    {
      if (localizationSettings == null)
        throw new ArgumentNullException(nameof(localizationSettings));

      if (localiztionOptions == null)
        throw new ArgumentNullException(nameof(localiztionOptions));

      var cookieProvider = GetLocalizationCookieProvider(localiztionOptions);
      cookieProvider.CookieName = localizationSettings.LocalizationCookieName;
    }

    internal static RequestLocalizationOptions ConfigureLocalizationOptions(
      RequestLocalizationOptions localizationOptions,
      LocalizationSettings localizationSettings)
    {
      if (localizationOptions == null)
        throw new ArgumentNullException(nameof(localizationOptions));

      if (localizationSettings == null)
        throw new ArgumentNullException(nameof(localizationSettings));

      var supportedCultures = GetSupportedCultures(localizationSettings);

      localizationOptions.DefaultRequestCulture = new RequestCulture(localizationSettings.DefaultCulture);
      localizationOptions.SupportedCultures = supportedCultures;
      localizationOptions.SupportedUICultures = supportedCultures;

      LocalizationHelper.ConfigureLocalizationCookieProvider(
          localizationSettings,
          localizationOptions);

      return localizationOptions;
    }

    private static CookieRequestCultureProvider GetLocalizationCookieProvider(
        RequestLocalizationOptions opts) =>
      opts.RequestCultureProviders
      .OfType<CookieRequestCultureProvider>()
      .First();

    private static IList<CultureInfo> GetSupportedCultures(LocalizationSettings localizationSettings) =>
      localizationSettings.SupportedCultures.Select(x => new CultureInfo(x)).ToList();
  }
}
