using System.IO;
using Microsoft.Extensions.Configuration;

namespace AspNetBase.Presentation.App.Utils
{
    internal class ConfigHelper
    {
      static readonly object locker = new object();
      static IConfigurationRoot root;

      internal static IConfigurationRoot GetRoot()
      {
        lock(locker)
        {
          return root ?? (root = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json")
            .Build());
        }
      }
    }
}
