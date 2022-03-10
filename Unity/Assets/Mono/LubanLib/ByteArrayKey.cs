using System;

namespace Bright.Net
{
    public struct ByteArrayKey : IEquatable<ByteArrayKey>
    {
        public byte[] Bytes { get; }
        public int Start { get; }
        public int Count { get; }

        private int _hashCode;


        public ByteArrayKey(byte[] bytes, int start, int count)
        {
            Bytes = bytes;
            Start = start;
            Count = count;
            _hashCode = GenHashCode(bytes, start, count);
        }

        public ByteArrayKey(byte[] bytes, int start, int count, int hashCode)
        {
            Bytes = bytes;
            Start = start;
            Count = count;
            _hashCode = hashCode;
        }

        public static ByteArrayKey Create(byte[] bytes, int start, int count)
        {
            return new ByteArrayKey(bytes, start, count);
        }

        public unsafe bool Equals(ByteArrayKey other)
        {
            if (this.Count != other.Count)
            {
                return false;
            }

            fixed (byte* p1 = &this.Bytes[Start], p2 = &other.Bytes[other.Start])
            {
                for (int i = 0; i < Count; i++)
                {
                    if (p1[i] != p2[i])
                    {
                        return false;
                    }
                }
                return true;
            }
        }

        public override bool Equals(object obj)
        {
            if (!(obj is ByteArrayKey))
            {
                return false;
            }

            return Equals((ByteArrayKey)obj);

        }

        public override int GetHashCode()
        {
            return _hashCode;
        }

        private static unsafe int GenHashCode(byte[] bytes, int start, int count)
        {
            fixed (byte* p = &bytes[start])
            {
                var hash = 17;
                for (var i = 0; i < count; i++)
                {
                    hash = hash * 23 + p[i];
                }
                return hash;
            }
        }

        public static bool operator ==(ByteArrayKey left, ByteArrayKey right)
        {
            return left.Equals(right);
        }

        public static bool operator !=(ByteArrayKey left, ByteArrayKey right)
        {
            return !(left == right);
        }
    }
}
