namespace YetAnotherEqualityComparer.Tests
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using Xunit;

    public class MyOldType
    {
        public int? ImportantNullableInt { get; set; }

        public DateTime ImportantDate { get; set; }

        public string ImportantString { get; set; }

        public string NotImportantString { get; set; }

        public override bool Equals(object obj)
        {
            if (object.ReferenceEquals(this, obj))
            {
                return true;
            }

            var other = obj as MyOldType;

            if (other == null)
            {
                return false;
            }

            return this.ImportantNullableInt == other.ImportantNullableInt
                && this.ImportantDate == other.ImportantDate
                && string.Equals(this.ImportantString, other.ImportantString, StringComparison.InvariantCultureIgnoreCase);
        }

        public override int GetHashCode()
        {
            return ImportantNullableInt.GetHashCode()
                ^ ImportantDate.GetHashCode()
                ^ StringComparer.InvariantCultureIgnoreCase.GetHashCode(this.ImportantString);
        }
    }

    public class MyNewType
    {
        private static readonly MemberEqualityComparer<MyNewType> Comparer = new MemberEqualityComparer<MyNewType>()
            .Add(o => o.ImportantNullableInt)
            .Add(o => o.ImportantDate)
            .Add(o => o.ImportantString, StringComparer.InvariantCultureIgnoreCase);

        public int? ImportantNullableInt { get; set; }

        public DateTime ImportantDate { get; set; }

        public string ImportantString { get; set; }

        public string NotImportantString { get; set; }

        public override bool Equals(object obj)
        {
            return Comparer.Equals(this, obj);
        }

        public override int GetHashCode()
        {
            return Comparer.GetHashCode(this);
        }
    }

    public class Demo
    {
        [Fact]
        public void DoDemo()
        {
            var oldX = new MyOldType
            {
                ImportantNullableInt = 50,
                ImportantDate = new DateTime(2000, 1, 1, 0, 0, 0),
                ImportantString = "test",
                NotImportantString = "does not matter"
            };

            var oldY = new MyOldType
            {
                ImportantNullableInt = 50,
                ImportantDate = new DateTime(2000, 1, 1, 0, 0, 0),
                ImportantString = "TEST",
                NotImportantString = "still does not matter"
            };

            Assert.True(oldX.Equals(oldY));

            Assert.Equal(oldX.GetHashCode(), oldY.GetHashCode());

            var newX = new MyNewType
            {
                ImportantNullableInt = 50,
                ImportantDate = new DateTime(2000, 1, 1, 0, 0, 0),
                ImportantString = "test",
                NotImportantString = "does not matter"
            };

            var newY = new MyNewType
            {
                ImportantNullableInt = 50,
                ImportantDate = new DateTime(2000, 1, 1, 0, 0, 0),
                ImportantString = "TEST",
                NotImportantString = "still does not matter"
            };

            Assert.True(newX.Equals(newY));

            Assert.Equal(newX.GetHashCode(), newY.GetHashCode());

            Assert.True(EqualityComparer<MyNewType>.Default.Equals(null, null));
        }
    }
}
