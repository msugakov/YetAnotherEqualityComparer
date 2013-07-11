namespace YetAnotherEqualityComparer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class MemberEqualityComparerTests
    {
        [Fact]
        public void TestEqualsAnonymousObject()
        {
            var a = new
                {
                    Length = 18,
                    Height = 4,
                    Color = 5
                };

            var b = new
                {
                    Length = 18,
                    Height = 4,
                    Color = 255
                };

            var c = new
                {
                    Length = 0,
                    Height = -500,
                    Color = 5
                };

            var comparer = MemberEqualityComparerFactory.Create(a)
                .Add(o => o.Length)
                .Add(o => o.Height);

            TestEqualsImpl(comparer, a, b, true);
            TestEqualsImpl(comparer, a, c, false);
            TestEqualsImpl(comparer, b, c, false);
            TestEqualsImpl(comparer, a, null, false);
            TestEqualsImpl(comparer, null, c, false);
            TestEqualsImpl(comparer, null, null, true);
        }

        [Fact]
        public void TestEqualsDateTime()
        {
            var comparer = new MemberEqualityComparer<DateTime>()
                .Add(dt => dt.Year)
                .Add(dt => dt.Day);

            TestEqualsImpl(comparer, new DateTime(2013, 7, 11), new DateTime(2013, 6, 11), true);
            TestEqualsImpl(comparer, new DateTime(2012, 7, 11), new DateTime(2013, 7, 11), false);
            TestEqualsImpl(comparer, new DateTime(2013, 7, 11), new DateTime(2013, 7, 2), false);
            TestEqualsImpl(comparer, new DateTime(2012, 1, 11), new DateTime(2012, 12, 11), true);
            TestEqualsImpl(comparer, new DateTime(2013, 7, 11, 23, 59, 59), new DateTime(2013, 7, 11, 1, 1, 0), true);
            TestEqualsImpl(comparer, new DateTime(), DateTime.Now, false);
        }

        [Fact]
        public void TestEqualsNullableTimeSpan()
        {
            var comparer = new MemberEqualityComparer<TimeSpan?>()
                .Add(ts => ts.Value.Hours)
                .Add(ts => ts.Value.Minutes);

            TestEqualsImpl(comparer, null, null, true);
            TestEqualsImpl(comparer, null, new TimeSpan(1000), false);
            TestEqualsImpl(comparer, null, new TimeSpan(), false);
            TestEqualsImpl(comparer, new TimeSpan(1000), new TimeSpan(), true);
            TestEqualsImpl(comparer, new TimeSpan(24, 0, 0), null, false);
            TestEqualsImpl(comparer, new TimeSpan(0, 11, 12, 13), new TimeSpan(1, 11, 12, 59), true);
        }

        [Fact]
        public void TestEqualsObject()
        {
            var comparer = new MemberEqualityComparer<object>()
                .Add(o => o.GetHashCode());

            var a = new object();
            var b = new object();

            TestEqualsImpl(comparer, a, b, false);
            TestEqualsImpl(comparer, b, a, false);
            TestEqualsImpl(comparer, a, a, true);
            TestEqualsImpl(comparer, b, b, true);
        }

        [Fact]
        public void TestEqualsString()
        {
            var comparer = new MemberEqualityComparer<string>()
                .Add(s => s.Length)
                .Add(s => s.LastOrDefault());

            TestEqualsImpl(comparer, string.Empty, string.Empty, true);
            TestEqualsImpl(comparer, "oranges", "bananas", true);
            TestEqualsImpl(comparer, "apples", "potatoes", false);
            TestEqualsImpl(comparer, "ooo", "aaa", false);
        }

        public void TestEqualsImpl<T>(MemberEqualityComparer<T> comparer, T x, T y, bool expectedResult)
        {
            Assert.Equal(expectedResult, comparer.Equals(x, y));

            if (expectedResult)
            {
                Assert.Equal(comparer.GetHashCode(x), comparer.GetHashCode(y));
            }
        }

        [Fact]
        public void TestGetHashCode()
        {
            var comparer = new MemberEqualityComparer<int?>()
                .Add(i => i.HasValue)
                .Add(i => i.Value);

            int? value = 6;

            Assert.NotEqual(value.GetHashCode(), comparer.GetHashCode(value));

            Assert.NotEqual(value.Value.GetHashCode(), comparer.GetHashCode(value));
        }

        [Fact]
        public void TestMutableHashCodeDefaultBehavior()
        {
            MutableTestType obj;

            MemberEqualityComparer<MutableTestType> defaultComparer;

            SetupMutalbeTestTypeComparer(GetHashCodeBehavior.Default, out obj, out defaultComparer);

            int hashCode = defaultComparer.GetHashCode(obj);

            obj.Weight += 1;

            int newHashCode = defaultComparer.GetHashCode(obj);

            Assert.NotEqual(hashCode, newHashCode);
        }

        [Fact]
        public void TestMutableHashCodeCacheBehavior()
        {
            MutableTestType obj;

            MemberEqualityComparer<MutableTestType> defaultComparer;

            SetupMutalbeTestTypeComparer(GetHashCodeBehavior.Cache, out obj, out defaultComparer);

            int hashCode = defaultComparer.GetHashCode(obj);

            obj.Weight += 1;

            int newHashCode = defaultComparer.GetHashCode(obj);

            Assert.Equal(hashCode, newHashCode);
        }

        [Fact]
        public void TestMutableHashCodeImmuatbleCheckBehavior()
        {
            MutableTestType obj;

            MemberEqualityComparer<MutableTestType> defaultComparer;

            SetupMutalbeTestTypeComparer(GetHashCodeBehavior.ImmutableCheck, out obj, out defaultComparer);

            int hashCode = defaultComparer.GetHashCode(obj);

            obj.Weight += 1;

            Assert.Throws<InvalidOperationException>(() => defaultComparer.GetHashCode(obj));
        }

        private static void SetupMutalbeTestTypeComparer(
            GetHashCodeBehavior getHashCodeBehavior,
            out MutableTestType obj,
            out MemberEqualityComparer<MutableTestType> comparer)
        {
            obj = new MutableTestType
            {
                Color = 2,
                Weight = 100
            };

            comparer = new MemberEqualityComparer<MutableTestType>(getHashCodeBehavior)
                .Add(o => o.Color)
                .Add(o => o.Weight);
        }

        internal class MutableTestType
        {
            public int Weight { get; set; }

            public int Color { get; set; }
        }
    }
}
