using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace FastStringExtensions
{
    public static class Extensions
    {
        private static Func<int, string> FastAllocateString = null;
        private unsafe delegate void MemcpyDelegate(byte* dest, byte* src, uint len);
        private static MemcpyDelegate Memcpy = null;

        static Extensions()
        {
            InitFastAllocateString();
            InitMemcpy();
        }

        private static void InitFastAllocateString()
        {
            var fasMethod = typeof(string).GetMethod("FastAllocateString", System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static);
            try
            {
                if (fasMethod != null) FastAllocateString = (Func<int, string>)fasMethod.CreateDelegate(typeof(Func<int, string>));
            }
            catch { }

            if (FastAllocateString == null) FastAllocateString = length => new string('\0', length);
        }

        private static void InitMemcpy()
        {
            var memcpys = typeof(Buffer).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Memmove");
            try
            {
                foreach (var f in memcpys)
                {
                    var pa = f.GetParameters();
                    if (pa.Length == 3 && pa[0].ParameterType == typeof(byte*) && pa[1].ParameterType == typeof(byte*) && pa[2].ParameterType == typeof(uint))
                    {
                        Memcpy = (MemcpyDelegate)f.CreateDelegate(typeof(MemcpyDelegate));
                        break;
                    }
                }
            }
            catch { }
            unsafe
            {
                if (Memcpy == null) Memcpy = SecondaryMemCpy;
            }
        }

        private static unsafe void SecondaryMemCpy(byte* dst, byte* src, uint length)
        {
            unchecked
            {
                if (length == 0) return;
                uint offset = 0;
                uint len4 = (uint)length >> 2;
                uint* from = (uint*)src;
                uint* to = (uint*)dst;
                for (uint i = 0; i < len4; ++i, offset += 4) to[i] = from[i];
                len4 = (uint)length % 4;
                for (uint i = 0; i < len4; ++i) dst[offset + i] = src[offset + i];
            }
        }


        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string _Substring(this string text, int startIndex)
        {
            return InternalSubstring(text, startIndex, text.Length - startIndex);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string _Substring(this string text, int startIndex, int length)
        {
            return InternalSubstring(text, startIndex, length);
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string _ToLower(this string text)
        {
            AssertNonNull(text);
            int slen = text.Length;
            var result = FastAllocateString(slen);
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _tp = result)
                    {
                        Memcpy((byte*)_tp, (byte*)_cp, (uint)text.Length << 1);

                        char* cp = _cp;
                        char* tp = _tp;
                        char* end = cp + slen;
                        int c;
                        while (cp < end)
                        {
                            c = *(cp++);
                            if (c >= 'A' && c <= 'Z')
                            {
                                *tp = (char)(c + 32);
                            }
                            ++tp;
                        }
                        return result;
                    }
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string _ToUpper(this string text)
        {
            AssertNonNull(text);
            int slen = text.Length;
            var result = FastAllocateString(slen);
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _tp = result)
                    {
                        Memcpy((byte*)_tp, (byte*)_cp, (uint)text.Length << 1);

                        char* cp = _cp;
                        char* tp = _tp;
                        char* end = cp + slen;
                        int c;
                        while (cp < end)
                        {
                            c = *(cp++);
                            if (c >= 'a' && c <= 'z')
                            {
                                *tp = (char)(c - 32);
                            }
                            ++tp;
                        }
                        return result;
                    }
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string _Trim(this string text)
        {
            AssertNonNull(text);
            int slen = text.Length;
            int currentIndex = 0;
            int lastIndex = slen - 1;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text)
                    {
                        for (; currentIndex < slen && _cp[currentIndex] == ' '; ++currentIndex) ;
                        for (; lastIndex >= currentIndex && _cp[lastIndex] == ' '; --lastIndex) ;
                        return InternalSubstring(text, currentIndex, lastIndex - currentIndex + 1);
                    }
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string _TrimStart(this string text)
        {
            AssertNonNull(text);
            int slen = text.Length;
            int currentIndex = 0;
            int lastIndex = slen - 1;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text)
                    {
                        for (; currentIndex < slen && _cp[currentIndex] == ' '; ++currentIndex) ;
                        return InternalSubstring(text, currentIndex, lastIndex - currentIndex + 1);
                    }
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static string _TrimEnd(this string text)
        {
            AssertNonNull(text);
            int slen = text.Length;
            int currentIndex = 0;
            int lastIndex = slen - 1;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text)
                    {
                        for (; lastIndex >= currentIndex && _cp[lastIndex] == ' '; --lastIndex) ;
                        return InternalSubstring(text, currentIndex, lastIndex - currentIndex + 1);
                    }
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int _CompareTo(this string text, string other)
        {
            AssertNonNull(text);
            if (other == null) return 1;
            int slen = text.Length;
            int olen = other.Length;
            int minLen = slen < olen ? slen : olen;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _op = other)
                    {
                        if (_cp == _op) return 0;
                        int j = 0;
                        while (j < minLen && _cp[j] == _op[j]) ++j;
                        if (j < minLen)
                        {
                            if (_cp[j] < _op[j]) return -1;
                            if (_cp[j] > _op[j]) return 1;
                        }
                        if (slen == olen) return 0;
                        if (slen < olen) return -1;
                        return 1;
                    }
                }
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int _ComparePart(this string text, int textStartIndex, string other, int otherStartIndex, int length)
        {
            AssertNonNull(text);
            if (other == null) return 1;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _dp = other)
                    {
                        char* cp = _cp + textStartIndex;
                        char* dp = _dp + otherStartIndex;
                        int minLen = length;
                        int less = 0;
                        if (text.Length - textStartIndex < minLen)
                        {
                            minLen = text.Length - textStartIndex;
                            less = -1;
                        }
                        if (other.Length - otherStartIndex < minLen)
                        {
                            minLen = other.Length - otherStartIndex;
                            less = 1;
                        }
                        int n = 0;
                        for (; n < minLen && cp[n] == dp[n]; ++n) ;
                        return n == minLen ? less : (cp[n] < dp[n] ? -1 : 1);
                    }
                }
            }

        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static List<string> _SplitToStrings(this string text, string delimiter)
        {
            AssertNonNull(text);
            if (string.IsNullOrEmpty(delimiter)) delimiter = " ";
            int slen = text.Length;
            int dlen = delimiter.Length;
            int diff = slen - dlen + 1;
            var result = new List<string>(slen);
            int currentIndex = 0;
            int startIndex = 0;
            int j, length;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _dp = delimiter)
                    {
                        for (j = 0, length = 0; j < dlen && _cp[currentIndex + j] == _dp[j]; ++j, ++length) ;
                        if (length == dlen)
                        {
                            currentIndex = startIndex = dlen;
                        }
                        while (currentIndex < diff)
                        {
                            for (j = 0, length = 0; j < dlen && _cp[currentIndex + j] == _dp[j]; ++j, ++length) ;
                            if (length == dlen)
                            {
                                if (currentIndex > startIndex) result.Add(new string(_cp, startIndex, currentIndex - startIndex));
                                currentIndex = startIndex = currentIndex + dlen;
                            }
                            else ++currentIndex;
                        }
                        if (currentIndex > startIndex)
                        {
                            result.Add(new string(_cp, startIndex, currentIndex - startIndex));
                        }
                    }
                }
            }
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static List<int> _SplitToInts(this string text, string delimiter)
        {
            AssertNonNull(text);
            if (string.IsNullOrEmpty(delimiter)) delimiter = " ";
            int slen = text.Length;
            int dlen = delimiter.Length;
            int diff = slen - dlen;
            var result = new List<int>(slen);
            int currentIndex = 0;
            int startIndex = 0;
            int j, length;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _dp = delimiter)
                    {
                        while (currentIndex < diff)
                        {
                            for (j = 0, length = 0; j < dlen && _cp[currentIndex + j] == _dp[j]; ++j, ++length) ;
                            if (length == dlen && currentIndex > startIndex)
                            {
                                result.Add(Convert.ToInt32(new string(_cp, startIndex, currentIndex - startIndex)));
                                currentIndex = startIndex = currentIndex + dlen - 1;
                            }
                            ++currentIndex;
                        }
                        if (currentIndex > startIndex)
                        {
                            result.Add(Convert.ToInt32(new string(_cp, startIndex, slen - startIndex)));
                        }
                    }
                }
            }
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static List<T> _SplitAndTransform<T>(this string text, string delimiter, Func<string, T> transform)
        {
            AssertNonNull(text);
            if (string.IsNullOrEmpty(delimiter)) delimiter = " ";
            int slen = text.Length;
            int dlen = delimiter.Length;
            int diff = slen - dlen;
            var result = new List<T>(slen);
            int currentIndex = 0;
            int startIndex = 0;
            int j, length;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _dp = delimiter)
                    {
                        while (currentIndex < diff)
                        {
                            for (j = 0, length = 0; j < dlen && _cp[currentIndex + j] == _dp[j]; ++j, ++length) ;
                            if (length == dlen && currentIndex > startIndex)
                            {
                                result.Add(transform(new string(_cp, startIndex, currentIndex - startIndex)));
                                currentIndex = startIndex = currentIndex + dlen - 1;
                            }
                            ++currentIndex;
                        }
                        if (currentIndex > startIndex)
                        {
                            result.Add(transform(new string(_cp, startIndex, slen - startIndex)));
                        }
                    }
                }
            }
            return result;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int _IndexOf(this string text, string part)
        {
            AssertNonNull(text);
            if (string.IsNullOrEmpty(part)) return -1;
            int slen = text.Length;
            int plen = part.Length;
            int diff = slen - plen;
            if (diff < 0) return -1;
            int currentIndex = 0;
            int j, length;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _dp = part)
                    {
                        char pfc = *_dp;
                        while (currentIndex < diff)
                        {
                            for (; currentIndex < slen && _cp[currentIndex] != pfc; ++currentIndex) ;
                            for (j = 0, length = 0; j < plen && _cp[currentIndex + j] == _dp[j]; ++j, ++length) ;
                            if (length == plen)
                            {
                                return currentIndex;
                            }
                            ++currentIndex;
                        }
                    }
                }
            }
            return -1;
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        public static int _LastIndexOf(this string text, string part)
        {
            AssertNonNull(text);
            if (string.IsNullOrEmpty(part)) return -1;
            int slen = text.Length;
            int plen = part.Length;
            int diff = slen - plen;
            if (diff < 0) return -1;
            int currentIndex = diff - 1;
            int j, length;
            unchecked
            {
                unsafe
                {
                    fixed (char* _cp = text, _dp = part)
                    {
                        char pfc = *_dp;
                        while (currentIndex >= 0)
                        {
                            for (; currentIndex >= 0 && _cp[currentIndex] != pfc; --currentIndex) ;
                            for (j = 0, length = 0; j < plen && _cp[currentIndex + j] == _dp[j]; ++j, ++length) ;
                            if (length == plen)
                            {
                                return currentIndex;
                            }
                            --currentIndex;
                        }
                    }
                }
            }
            return -1;
        }





        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static void AssertNonNull(string str)
        {
            if (str == null)
            {
                throw new NullReferenceException();
            }
        }

        [MethodImplAttribute(MethodImplOptions.AggressiveInlining)]
        private static string InternalSubstring(string text, int startIndex, int length)
        {
            AssertNonNull(text);
            unchecked
            {
                int textLength = text.Length;
                int remainingLength = textLength - startIndex;

                //Bounds Checking.
                if (startIndex < 0 || startIndex >= textLength)
                {
                    throw new ArgumentOutOfRangeException("startIndex");
                }

                if (remainingLength < 0 || remainingLength < length || length < 0)
                {
                    throw new ArgumentOutOfRangeException("length");
                }

                if (textLength == 0 || length == 0) return string.Empty;
                if (startIndex == 0 && length == textLength) return text;

                var result = FastAllocateString(length);
                unsafe
                {
                    fixed (char* cp = text, tp = result)
                    {
                        Memcpy((byte*)tp, (byte*)(cp + startIndex), (uint)length << 1);
                    }
                }
                return result;
            }
        }
    }
}
