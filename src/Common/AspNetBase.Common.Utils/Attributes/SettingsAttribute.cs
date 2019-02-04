using System;

namespace AspNetBase.Common.Utils.Attributes
{
  [AttributeUsage(AttributeTargets.Class)]
  public class SettingsAttribute : Attribute
  {
    public SettingsAttribute(string key)
    {
      if (string.IsNullOrWhiteSpace(key))
      {
        throw new ArgumentException("Invalid config settings key.", nameof(key));
      }

      this.Key = key;
    }

    public string Key { get; }
  }
}
