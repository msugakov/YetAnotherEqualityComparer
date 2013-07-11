namespace YetAnotherEqualityComparer.Tests.Auxiliary
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;
    using Xunit.Extensions;

    public class ValueTypeTests
    {
        internal delegate void TestDelegate();

        internal enum TestEnum
        {
        }

        internal interface ITestInterface
        {
        }

        internal struct TestStruct
        {
        }

        internal class TestClass
        {
        }

        [Theory]
        [InlineData(typeof(Enum), false, true)]
        [InlineData(typeof(Array), false, true)]
        [InlineData(typeof(int?), true, false)]
        [InlineData(typeof(int), true, false)]
        [InlineData(typeof(TestStruct), true, false)]
        [InlineData(typeof(DateTime), true, false)]
        [InlineData(typeof(TestEnum), true, false)]
        [InlineData(typeof(ITestInterface), false, false)]
        [InlineData(typeof(TestClass), false, true)]
        [InlineData(typeof(string), false, true)]
        [InlineData(typeof(int[]), false, true)]
        [InlineData(typeof(List<int>), false, true)]
        [InlineData(typeof(Action), false, true)]
        [InlineData(typeof(Func<object>), false, true)]
        [InlineData(typeof(TestDelegate), false, true)]
        public void TestIsValueType(Type type, bool isValueType, bool isClass)
        {
            Assert.Equal(isValueType, type.IsValueType);

            Assert.Equal(isClass, type.IsClass);
        }
    }
}
