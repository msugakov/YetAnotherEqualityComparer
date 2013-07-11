namespace YetAnotherEqualityComparer.Tests.Auxiliary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class DefaultEqualityComparerTests
    {
        [Fact]
        public void TestNullEquals()
        {
            bool equals = EqualityComparer<object>.Default.Equals(null, null);

            Assert.True(equals);

            equals = EqualityComparer<string>.Default.Equals(null, null);

            Assert.True(equals);
        }

        [Fact]
        public void TestNullGetHashCode()
        {
            int hashCode = EqualityComparer<object>.Default.GetHashCode(null);

            Assert.Equal(0, hashCode);

            hashCode = EqualityComparer<string>.Default.GetHashCode(null);

            Assert.Equal(0, hashCode);

            hashCode = EqualityComparer<DateTime>.Default.GetHashCode(default(DateTime));

            Assert.Equal(0, hashCode);
        }
    }
}
