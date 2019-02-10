using System;
using System.Collections.Concurrent;

namespace AspNetBase.Core.Settings
{
  /// <summary>
  /// Use the settings locator singleton only if settings access is needed during application startup.
  /// </summary>
  /// <remarks>
  /// Registering settings via static method <see cref="Set"> (i.e. initializing the locator) can be commented out for faster application startup.
  /// </remarks>
  public abstract class SettingsLocator
  {
    private static ConcurrentDictionary<Type, object> _settingsMap = new ConcurrentDictionary<Type, object>();

    public static TSetting Get<TSetting>() where TSetting : class
    {
      var typeKey = typeof(TSetting);
      _settingsMap.TryGetValue(typeKey, out object settingObj);

      if (settingObj == null)
        throw new InvalidOperationException($"Requested setting type '{typeKey.FullName}' instance was not found.");

      var setting = settingObj as TSetting;

      if (setting == null)
        throw new InvalidCastException($"The resolved setting type instance could not be cast into the reqeusted setting type '{typeKey.FullName}'.");

      return setting;
    }

    internal static void Set(Type settingType, object setting)
    {
      if (settingType == null)
        throw new ArgumentNullException(nameof(settingType));

      if (setting == null)
        throw new ArgumentNullException(nameof(setting));

      var added = _settingsMap.TryAdd(settingType, setting);

      if (!added)
        throw new InvalidOperationException(
          $"Setting instance of type '{settingType.FullName}' was not successful in the global settings locator.");
    }
  }
}
