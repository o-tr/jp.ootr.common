using UnityEngine;

namespace jp.ootr.common
{
    public static class ByteUtils
    {
        public static bool Similar(this byte[] data1, byte[] data2, float sampleRate = 0.5f)
        {
            Random.InitState((int)Time.deltaTime);
            var sampleSize = (int)(data1.Length * sampleRate);

            return SimilarInternal(data1, data2, sampleSize);
        }

        public static bool Similar(this byte[] data1, byte[] data2, int sampleSize)
        {
            return SimilarInternal(data1, data2, sampleSize);
        }

        private static bool SimilarInternal(byte[] data1, byte[] data2, int sampleSize)
        {
            if (data1 == null || data2 == null || data1.Length != data2.Length)
                return false;
            for (int i = 0; i < sampleSize; i++)
            {
                int x = Random.Range(0, data1.Length - 1);

                if (data1[x] != data2[x])
                    return false;
            }

            return true;
        }

        public static bool MayBlank(this byte[] data, float sampleRate = 0.5f)
        {
            Random.InitState((int)Time.deltaTime);
            var sampleSize = (int)(data.Length * sampleRate);

            return MayBlankInternal(data, sampleSize);
        }

        public static bool MayBlank(this byte[] data, int sampleSize)
        {
            return MayBlankInternal(data, sampleSize);
        }

        private static bool MayBlankInternal(byte[] data, int sampleSize)
        {
            if (data == null) return true;
            var len = data.Length / 4;
            for (int i = 0; i < sampleSize; i++)
            {
                int x = Random.Range(0, len - 1);

                if (data[x * 4 + 3] != 0)
                    return false;
            }

            return true;
        }
    }
}