namespace YetAnotherEqualityComparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Use factory to create <see cref="MemberEqualityComparer{T}"/> for anonymous types
    /// </summary>
    public class MemberEqualityComparerFactory
    {
        /// <summary>
        /// Create <see cref="MemberEqualityComparer{T}"/> based on type information of argument
        /// </summary>
        /// <typeparam name="T">Type of argument</typeparam>
        /// <param name="value">Value is not used just type information is significant</param>
        /// <returns>
        /// <see cref="MemberEqualityComparer{T}"/> instance.
        /// Don't forget to call <see cref="MemberEqualityComparer{T}.Add{TMember}(System.Func{T,TMember})"/>
        /// to configure members used for comparison.
        /// </returns>
        public static MemberEqualityComparer<T> Create<T>(T value)
        {
            return new MemberEqualityComparer<T>();
        }
    }
}
