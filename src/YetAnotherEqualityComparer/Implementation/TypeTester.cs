namespace YetAnotherEqualityComparer.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Linq.Expressions;
    using System.Reflection;
    using System.Text;

    public class TypeTester<T>
    {
        private static readonly TypeInfoContainer TypeInfo = new TypeInfoContainer(typeof(T));

        public static bool IsNull(T value)
        {
            if (!TypeInfo.IsValueType)
            {
                return object.ReferenceEquals(value, null);
            }

            if (TypeInfo.IsNullable)
            {
                return value == null;
            }

            return false;
        }

        public static bool ReferenceEquals(T x, T y)
        {
            return !TypeInfo.IsValueType && object.ReferenceEquals(x, y);
        }

        internal class TypeInfoContainer
        {
            public TypeInfoContainer(Type type)
            {
                IsValueType = type.IsValueType;

                IsNullable = Nullable.GetUnderlyingType(typeof(T)) != null;
            }

            public bool IsValueType { get; private set; }

            public bool IsNullable { get; private set; }
        }
    }
}
