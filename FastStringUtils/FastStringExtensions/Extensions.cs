using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FastStringExtensions
{
    public static class Extensions
    {
        private static Func<int, string> FastAllocateString = null;
        private unsafe delegate void MemcpyDelegate(byte* dest, byte* src, int len);
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
            var memcpys = typeof(Buffer).GetMethods(System.Reflection.BindingFlags.NonPublic | System.Reflection.BindingFlags.Static).Where(m => m.Name == "Memcpy");
            try
            {
                foreach (var f in memcpys)
                {
                    var pa = f.GetParameters();
                    if (pa.Length == 3 && pa[0].ParameterType == typeof(byte*) && pa[1].ParameterType == typeof(byte*) && pa[2].ParameterType == typeof(int)) Memcpy = (MemcpyDelegate)f.CreateDelegate(typeof(MemcpyDelegate));
                }
            }
            catch { }
            unsafe
            {
                if (Memcpy == null) Memcpy = SecondaryMemCpy;
            }
        }

        private static unsafe void SecondaryMemCpy(byte* dst, byte* src, int length)
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
}
