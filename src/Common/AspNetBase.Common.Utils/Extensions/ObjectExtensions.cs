using System;
using Newtonsoft.Json;

namespace AspNetBase.Common.Utils.Extensions
{
  public static class ObjectExtensions
  {
    static JsonSerializerSettings Settings = new JsonSerializerSettings
    {
      ReferenceLoopHandling = ReferenceLoopHandling.Ignore
    };

    public static string ToJson(this object objValue)
    {
      if (objValue == null)
        throw new ArgumentNullException(nameof(objValue));

      return JsonConvert.SerializeObject(objValue, Formatting.Indented, Settings);
    }
  }
}
