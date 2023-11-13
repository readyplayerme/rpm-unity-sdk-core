using System;
using System.Linq;

namespace ReadyPlayerMe.Core.Editor.PackageManager.Extensions
{
    public static class PackageManagerStringExtensions
    {
        public static string TryParsePackageUrl(this string packageId)
        {
            return packageId.Split(new char['@'], StringSplitOptions.RemoveEmptyEntries).LastOrDefault();
        }
    }
}
