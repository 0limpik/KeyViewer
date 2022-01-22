using System;
using System.Collections.Generic;
using System.Linq;
using KeyViewer.Services;

namespace KeyViewer.Extensions
{
    public static class KeysEnumExtensions
    {
        public static string DisplayName<T>(this T key, List<KeyParameters<T>> list) where T : Enum
        {
            var repositoryKey = list.FirstOrDefault(x => x.Key.Equals(key));

            return repositoryKey?.DisplayName;
        }
    }
}
