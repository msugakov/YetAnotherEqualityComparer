namespace YetAnotherEqualityComparer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;
    using YetAnotherEqualityComparer.Implementation;

    public class NullTesterTests
    {
        internal enum TestEnum
        {
            Value
        }

        [Fact]
        public void TestIsNull()
        {
            TestIsNullImpl<string>(null, true);

            TestIsNullImpl<string>("Am I overcomplicating things?", false);

            TestIsNullImpl<int?>(null, true);

            TestIsNullImpl<int?>(6, false);

            TestIsNullImpl<TestEnum?>(null, true);

            TestIsNullImpl<TestEnum?>(TestEnum.Value, false);

            TestIsNullImpl<Enum>(null, true);

            TestIsNullImpl<Enum>(TestEnum.Value, false);

            TestIsNullImpl<int>(default(int), false);

            TestIsNullImpl<int>(6, false);

            TestIsNullImpl<Action>(() => { }, false);

            TestIsNullImpl<Action>(null, true);
        }

        private void TestIsNullImpl<T>(T value, bool expectedResult)
        {
            Assert.Equal(expectedResult, NullTester<T>.IsNull(value));
        }
    }
}
