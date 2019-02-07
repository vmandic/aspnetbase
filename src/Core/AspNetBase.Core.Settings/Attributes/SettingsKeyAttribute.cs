using System;

namespace AspNetBase.Core.Settings.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class SettingsKeyAttribute : Attribute
  {
    public SettingsKeyAttribute(string keyName)
    {
      if (string.IsNullOrWhiteSpace(keyName))
      {
        throw new ArgumentException("Invalid configuration settings key name value.", nameof(keyName));
      }

      this.KeyName = keyName;
    }
    public string KeyName { get; }
  }
}
