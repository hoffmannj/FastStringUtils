using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace FastStringExtensions.Tests
{
    public class Tests
    {
        private const string TEXT = "    Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua       ";
        private const string TEXT_2 = "    Lorem ipsum dolor sit omet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua       ";
        private const string NUMBERS = "1, 43, 11, 2";

        [Fact]
        public void Test_Substring_1()
        {
            var original = TEXT.Substring(5);
            var fast = TEXT._Substring(5);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_Substring_1_Exception_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._Substring(-1);
            });
        }

        [Fact]
        public void Test_Substring_1_Exception_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._Substring(TEXT.Length + 5);
            });
        }

        [Fact]
        public void Test_Substring_1_Exception_3()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._Substring(5);
            });
        }

        [Fact]
        public void Test_Substring_2()
        {
            var original = TEXT.Substring(5, 15);
            var fast = TEXT._Substring(5, 15);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_Substring_2_Exception_1()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._Substring(-1, 10);
            });
        }

        [Fact]
        public void Test_Substring_2_Exception_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._Substring(5, -1);
            });
        }

        [Fact]
        public void Test_Substring_2_Exception_3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._Substring(TEXT.Length - 5, 10);
            });
        }

        [Fact]
        public void Test_Substring_2_Exception_4()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._Substring(5, 5);
            });
        }

        [Fact]
        public void Test_ToLower()
        {
            var original = TEXT.ToLower();
            var fast = TEXT._ToLower();
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_ToLower_NULL()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._ToLower();
            });
        }

        [Fact]
        public void Test_ToUpper()
        {
            var original = TEXT.ToUpper();
            var fast = TEXT._ToUpper();
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_ToUpper_NULL()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._ToUpper();
            });
        }

        [Fact]
        public void Test_Trim()
        {
            var original = TEXT.Trim();
            var fast = TEXT._Trim();
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_Trim_NULL()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._Trim();
            });
        }

        [Fact]
        public void Test_TrimStart()
        {
            var original = TEXT.TrimStart();
            var fast = TEXT._TrimStart();
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_TrimStart_NULL()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._TrimStart();
            });
        }

        [Fact]
        public void Test_TrimEnd()
        {
            var original = TEXT.TrimEnd();
            var fast = TEXT._TrimEnd();
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_TrimEnd_NULL()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._TrimEnd();
            });
        }

        [Fact]
        public void Test_CompareTo_1()
        {
            var original = TEXT.CompareTo(TEXT);
            var fast = TEXT._CompareTo(TEXT);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_CompareTo_2()
        {
            var original = TEXT.CompareTo(TEXT_2);
            var fast = TEXT._CompareTo(TEXT_2);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_CompareTo_3()
        {
            var original = TEXT.CompareTo(null);
            var fast = TEXT._CompareTo(null);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_CompareTo_Exception_1()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._CompareTo(TEXT);
            });
        }

        [Fact]
        public void Test_ComparePart_1()
        {
            var original = TEXT.Substring(10, 10).CompareTo(TEXT.Substring(10, 10));
            var fast = TEXT._ComparePart(10, TEXT, 10, 10);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_ComparePart_2()
        {
            var original = TEXT.Substring(10, 10).CompareTo(TEXT_2.Substring(10, 10));
            var fast = TEXT._ComparePart(10, TEXT_2, 10, 10);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_ComparePart_3()
        {
            string s = null;
            var original = TEXT.Substring(10, 10).CompareTo(s);
            var fast = TEXT._ComparePart(10, s, 10, 10);
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_ComparePart_Exception_1()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._ComparePart(10, TEXT, 10, 10);
            });
        }

        [Fact]
        public void Test_Contains_1()
        {
            var original = TEXT.Contains("dolore magna aliqua       ");
            var fast = TEXT._Contains("dolore magna aliqua       ");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_Contains_2()
        {
            var original = TEXT.Contains("something");
            var fast = TEXT._Contains("something");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_Contains_Exception_1()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._Contains("em");
            });
        }

        [Fact]
        public void Test_Contains_Exception_2()
        {
            Assert.Throws<ArgumentNullException>(() =>
            {
                string s = null;
                var fast = TEXT._Contains(s);
            });
        }

        [Fact]
        public void Test_SplitToStrings_1()
        {
            var original = TEXT.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
            var fast = TEXT._SplitToStrings(null);
            Assert.True(original.Length == fast.Count);
            Assert.True(original.SequenceEqual(fast));
        }

        [Fact]
        public void Test_SplitToStrings_2()
        {
            var original = TEXT.Split(new string[] { "em" }, StringSplitOptions.RemoveEmptyEntries);
            var fast = TEXT._SplitToStrings("em");
            Assert.True(original.Length == fast.Count);
            Assert.True(original.SequenceEqual(fast));
        }

        [Fact]
        public void Test_SplitToStrings_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._SplitToStrings("em");
            });
        }

        [Fact]
        public void Test_SplitToInts()
        {
            var original = NUMBERS.Split(new string[] { ", " }, StringSplitOptions.RemoveEmptyEntries).Select(s => int.Parse(s)).ToArray();
            var fast = NUMBERS._SplitToInts(", ");
            Assert.True(original.Length == fast.Count);
            Assert.True(original.SequenceEqual(fast));
        }

        [Fact]
        public void Test_SplitToInts_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._SplitToInts(", ");
            });
        }

        [Fact]
        public void Test_SplitAndTransform()
        {
            var original = TEXT.Split(new string[] { "em" }, StringSplitOptions.RemoveEmptyEntries).Select(s => new string(s.Reverse().ToArray())).ToArray();
            var fast = TEXT._SplitAndTransform("em", s => new string(s.Reverse().ToArray()));
            Assert.True(original.Length == fast.Count);
            Assert.True(original.SequenceEqual(fast));
        }

        [Fact]
        public void Test_SplitAndTransform_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._SplitAndTransform("em", q => new string(q.Reverse().ToArray()));
            });
        }

        [Fact]
        public void Test_IndexOf_1()
        {
            var original = TEXT.IndexOf("em");
            var fast = TEXT._IndexOf("em");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_IndexOf_2()
        {
            var original = TEXT.IndexOf("window");
            var fast = TEXT._IndexOf("window");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_IndexOf_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._IndexOf("em");
            });
        }

        [Fact]
        public void Test_LastIndexOf_1()
        {
            var original = TEXT.LastIndexOf("em");
            var fast = TEXT._LastIndexOf("em");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_LastIndexOf_2()
        {
            var original = TEXT.IndexOf("window");
            var fast = TEXT._LastIndexOf("window");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_LastIndexOf_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._LastIndexOf("em");
            });
        }

        [Fact]
        public void Test_StartsWith_1()
        {
            var original = TEXT.StartsWith("em");
            var fast = TEXT._StartsWith("em");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_StartsWith_2()
        {
            var original = TEXT.StartsWith("    Lorem ipsum ");
            var fast = TEXT._StartsWith("    Lorem ipsum ");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_StartsWith_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._StartsWith("em");
            });
        }

        [Fact]
        public void Test_EndsWith_1()
        {
            var original = TEXT.EndsWith("em");
            var fast = TEXT._EndsWith("em");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_EndsWith_2()
        {
            var original = TEXT.EndsWith("    Lorem ipsum ");
            var fast = TEXT._EndsWith("    Lorem ipsum ");
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_EndsWith_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._EndsWith("em");
            });
        }

        [Fact]
        public void Test_ToArray_1()
        {
            var original = TEXT.ToCharArray();
            var fast = TEXT._ToCharArray();
            Assert.Equal(original.Length, fast.Length);
            Assert.True(original.SequenceEqual(fast));
        }

        [Fact]
        public void Test_ToArray_2()
        {
            var original = TEXT.ToCharArray(10, 10);
            var fast = TEXT._ToCharArray(10, 10);
            Assert.Equal(original.Length, fast.Length);
            Assert.True(original.SequenceEqual(fast));
        }

        [Fact]
        public void Test_ToArray_3()
        {
            var original = TEXT.ToCharArray(10, 0);
            var fast = TEXT._ToCharArray(10, 0);
            Assert.Equal(original.Length, fast.Length);
            Assert.True(original.SequenceEqual(fast));
        }

        [Fact]
        public void Test_ToArray_Exception_1()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._ToCharArray();
            });
        }

        [Fact]
        public void Test_ToArray_Exception_2()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._ToCharArray(-1, 10);
            });
        }

        [Fact]
        public void Test_ToArray_Exception_3()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._ToCharArray(10, 1000);
            });
        }

        [Fact]
        public void Test_ToArray_Exception_4()
        {
            Assert.Throws<ArgumentOutOfRangeException>(() =>
            {
                var fast = TEXT._ToCharArray(10, -1);
            });
        }

        [Fact]
        public void Test_Reverse()
        {
            var original = new string(TEXT.Reverse().ToArray());
            var fast = TEXT._Reverse();
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_Reverse_Exception()
        {
            string s = null;
            Assert.Throws<NullReferenceException>(() =>
            {
                var fast = s._Reverse();
            });
        }

        /*[Fact]
        public void Test_Substring_speed()
        {
            var sw = new Stopwatch();
            sw.Start();
            sw.Stop();
            sw.Reset();

            string original = string.Empty, fast = string.Empty;

            sw.Start();
            for (int i = 0; i < 1000000; ++i)
            {
                original = TEXT.Substring(10);
            }
            sw.Stop();
            var elapsed_original = sw.ElapsedMilliseconds;
            sw.Reset();
            sw.Start();
            for (int i = 0; i < 1000000; ++i)
            {
                fast = TEXT._Substring(10);
            }
            sw.Stop();
            var elapsed_fast = sw.ElapsedMilliseconds;
            Assert.Equal(original, fast);
            Assert.True(elapsed_fast < elapsed_original);
        }*/
    }
}
