using JetBrains.Annotations;
using VRC.SDKBase;

namespace jp.ootr.common
{
    public static class Network
    {
        public static bool IsInsecureUrl([CanBeNull] this string url)
        {
            if (url == null) return true;
            return !url.StartsWith("https://");
        }

        public static bool IsInsecureUrl([CanBeNull] this VRCUrl url)
        {
            if (url == null) return true;
            return IsInsecureUrl(url.ToString());
        }

        public static bool IsValidUrl([CanBeNull] this string url)
        {
            if (url == null) return false;
            return url.StartsWith("https://");
        }

        public static bool IsValidUrl([CanBeNull] this VRCUrl url)
        {
            if (url == null) return false;
            return IsValidUrl(url.ToString());
        }

        public static bool IsValidLocalUrl([CanBeNull] this string url)
        {
            if (url == null) return false;
            return url.StartsWith("file://");
        }

        public static bool IsValidLocalUrl([CanBeNull] this VRCUrl url)
        {
            if (url == null) return false;
            return IsValidLocalUrl(url.ToString());
        }

        [CanBeNull]
        public static VRCUrl FindVrcUrlByUrl([CanBeNull] [ItemCanBeNull] VRCUrl[] vrcUrls, [CanBeNull] string url)
        {
            if (vrcUrls == null || url == null) return null;
            foreach (var vrcUrl in vrcUrls)
            {
                if (vrcUrl == null) continue;
                if (url == vrcUrl.ToString())
                    return vrcUrl;
            }

            return null;
        }

        [NotNull]
        public static string[] ToStrings([CanBeNull] [ItemCanBeNull] this VRCUrl[] urls)
        {
            if (urls == null) return new string[0];
            var strings = new string[urls.Length];
            for (var i = 0; i < urls.Length; i++)
            {
                if (urls[i] == null)
                {
                    strings[i] = "";
                    continue;
                }

                strings[i] = urls[i].ToString();
            }

            return strings;
        }
    }
}
