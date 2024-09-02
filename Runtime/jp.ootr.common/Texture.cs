using UnityEngine;
using VRC.SDKBase;

namespace jp.ootr.common
{
    public static class TextureUtils
    {
        public static Texture2D Copy(this Texture texture, bool flipHorizontal = false, bool flipVertical = false)
        {
            var tmpTexture = (Texture2D)texture;
            if (tmpTexture == null) return null;
            return tmpTexture.Copy(flipHorizontal, flipVertical);
        }

        public static Texture2D Copy(this Texture2D texture, bool flipHorizontal = false, bool flipVertical = false)
        {
            var tmpTexture = new RenderTexture(texture.width, texture.height, 0, RenderTextureFormat.ARGB32,
                RenderTextureReadWrite.Default);
            tmpTexture.Create();
            VRCGraphics.Blit(texture, tmpTexture, new Vector2(flipHorizontal ? -1 : 1, flipVertical ? -1 : 1),
                new Vector2(flipHorizontal ? 1 : 0, flipVertical ? 1 : 0));
            var readableText = new Texture2D(texture.width, texture.height);
            readableText.ReadPixels(new Rect(0, 0, tmpTexture.width, tmpTexture.height), 0, 0);
            readableText.Apply();
            tmpTexture.Release();
            return readableText;
        }

        public static bool Similar(this Texture2D texture1, Texture2D texture2, float sampleRate = 0.5f)
        {
            Random.InitState(Time.deltaTime.GetHashCode());
            var sampleSize = (int)(texture1.width * texture1.height * sampleRate);

            return SimilarInternal(texture1, texture2, sampleSize);
        }

        public static bool Similar(this Texture2D texture1, Texture2D texture2, int sampleSize)
        {
            return SimilarInternal(texture1, texture2, sampleSize);
        }

        private static bool SimilarInternal(Texture2D texture1, Texture2D texture2, int sampleSize)
        {
            if (texture1 == null || texture2 == null || texture1.width != texture2.width ||
                texture1.height != texture2.height)
                return false;
            for (var i = 0; i < sampleSize; i++)
            {
                var x = Random.Range(0, texture1.width - 1);
                var y = Random.Range(0, texture1.height - 1);

                if (texture1.GetPixel(x, y) != texture2.GetPixel(x, y))
                    return false;
            }

            return true;
        }
    }
}
