YetAnotherEqualityComparer
==========================
Yet Another member Equality Comparer for .NET

## Problem ##
Do you often have to override `Equals` and `GetHashCode` in order to compare objects based on its members?

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

If you've done this a number of times you might feel that enumerating the same fields in both methods is not the right thing. Maintaining the same list of members in both methods as type grows may be a bit of pain. And imagine consequences if in `GetHashCode` you've mistaken and there's used hash code of `NotImportantString` instead of `ImportantString` or you forgot to take case-invariant hash code.

## Solution ##
This small library allows you to setup members list only once per type and have `Equals` and `GetHashCode` implemented in terms of this setup.

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

That's all you need to do, `MemberEqualityComparer` takes care about the rest.

Additionally `MemberEqualityComparer<T>` acts as `IEqualityComparer<T>` so you may use it to compare types you can't change, value types (`DateTime`, `TimeSpan`, nullables and your own structs) and anonymous types.

## Alternatives ##

There also exist more advanced libraries able to provide not only `IEqualityComparer<T>` but `IComparer<T>` also. Be sure to check those too:
1. [ComparerExtensions](https://github.com/jehugaleahsa/ComparerExtensions)
2. [Comparers (by Stephen Cleary)](http://comparers.codeplex.com/)

## Installation ##
The package is available as NuGet https://www.nuget.org/packages/YetAnotherEqualityComparer/
