namespace YetAnotherEqualityComparer.Tests.Auxiliary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class IsOperatorTests
    {
        [Fact]
        public void NullIsType()
        {
            object a = null;

            Assert.False(a is object);
        }
    }
}
