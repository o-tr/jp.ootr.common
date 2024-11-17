using UnityEngine;
using VRC.SDKBase;

namespace jp.ootr.common
{
    public static class ByteUtils
    {
        public static bool Similar(this byte[] data1, byte[] data2, float sampleRate = 0.5f)
        {
            if (!Utilities.IsValid(data1) || !Utilities.IsValid(data2) || data1.Length != data2.Length)
                return false;
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
            if (!Utilities.IsValid(data1) || !Utilities.IsValid(data2) || data1.Length != data2.Length)
                return false;
            for (var i = 0; i < sampleSize; i++)
            {
                var x = Random.Range(0, data1.Length - 1);

                if (data1[x] != data2[x])
                    return false;
            }

            return true;
        }

        public static bool MayBlank(this byte[] data, float sampleRate = 0.5f)
        {
            if (!Utilities.IsValid(data)) return true;
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
            if (!Utilities.IsValid(data)) return true;
            var len = data.Length / 4;
            for (var i = 0; i < sampleSize; i++)
            {
                var x = Random.Range(0, len - 1);

                if (data[x * 4 + 3] != 0)
                    return false;
            }

            return true;
        }
    }
}
