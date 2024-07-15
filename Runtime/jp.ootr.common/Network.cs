using VRC.SDKBase;

namespace jp.ootr.common
{
    public static class Network
    {
        public static bool IsInsecureUrl(this string url)
        {
            return url.StartsWith("http://");
        }

        public static bool IsInsecureUrl(this VRCUrl url)
        {
            return IsInsecureUrl(url.ToString());
        }

        public static bool IsValidUrl(this string url)
        {
            return url.StartsWith("https://");
        }

        public static bool IsValidUrl(this VRCUrl url)
        {
            return IsValidUrl(url.ToString());
        }

        public static VRCUrl FindVrcUrlByUrl(VRCUrl[] vrcUrls, string url)
        {
            foreach (var vrcUrl in vrcUrls)
                if (url == vrcUrl.ToString())
                    return vrcUrl;

            return null;
        }

        public static string[] ToStrings(this VRCUrl[] urls)
        {
            var strings = new string[urls.Length];
            for (var i = 0; i < urls.Length; i++) strings[i] = urls[i].ToString();
            return strings;
        }
    }
}