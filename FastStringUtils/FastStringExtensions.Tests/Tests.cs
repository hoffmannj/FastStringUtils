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
        public void Test_TrimStart()
        {
            var original = TEXT.TrimStart();
            var fast = TEXT._TrimStart();
            Assert.Equal(original, fast);
        }

        [Fact]
        public void Test_TrimEnd()
        {
            var original = TEXT.TrimEnd();
            var fast = TEXT._TrimEnd();
            Assert.Equal(original, fast);
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
