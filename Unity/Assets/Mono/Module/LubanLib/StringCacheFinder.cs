using System;
using System.Collections.Generic;
using System.Text;
using Bright.Net;
using UnityEngine;

namespace Bright.Serialization
{
    /// <summary>
    /// 反序列化字符串缓存类
    /// </summary>
    public static class StringCacheFinder
    {
        [RuntimeInitializeOnLoadMethod]
        private static void RegisterFunc()
        {
            ByteBuf.StringCacheFinder = GetString;
        }

        private static readonly Dictionary<ByteArrayKey, string> BytesStringMap = new Dictionary<ByteArrayKey, string>();

        private const int MaxCacheBytesLength = 128;

        private static string GetString(byte[] bytes, int readIndex, int count)
        {
            if (count > MaxCacheBytesLength)
            {
                return Encoding.UTF8.GetString(bytes, readIndex, count);
            }

            ByteArrayKey byteArray = new ByteArrayKey(bytes, readIndex, count);
            if (BytesStringMap.TryGetValue(byteArray, out string strValue))
            {
                return strValue;
            }

            strValue = Encoding.UTF8.GetString(bytes, readIndex, count);
            byte[] buffer = new byte[count];
            Buffer.BlockCopy(bytes, readIndex, buffer, 0, count);
            ByteArrayKey bufferByteArrayKey = new ByteArrayKey(buffer, 0, count, byteArray.GetHashCode());
            BytesStringMap.Add(bufferByteArrayKey, strValue);
            return strValue;
        }

        public static void ClearCache()
        {
            BytesStringMap.Clear();
        }
    }
}