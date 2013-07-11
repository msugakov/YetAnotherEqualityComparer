namespace YetAnotherEqualityComparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public class MemberEqualityComparerFactory
    {
        public static MemberEqualityComparer<T> Create<T>(T value)
        {
            return new MemberEqualityComparer<T>();
        }
    }
}
