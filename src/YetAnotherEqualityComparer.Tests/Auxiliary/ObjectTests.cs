namespace YetAnotherEqualityComparer.Tests.Auxiliary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class ObjectTests
    {
        [Fact]
        public void TestReferenceEquals()
        {
            var a = new object();

            Assert.False(object.ReferenceEquals(null, a));

            Assert.False(object.ReferenceEquals(a, null));

            Assert.True(object.ReferenceEquals(null, null));
        }
    }
}
