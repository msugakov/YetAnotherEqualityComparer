namespace YetAnotherEqualityComparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    /// <summary>
    /// Controls behavior of GetHashCode function.
    /// </summary>
    public enum GetHashCodeBehavior
    {
        /// <summary>
        /// Compute hash function each call.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use cached hash code value in subsequent calls.
        /// The type must be immutable or there will be unexpected behavior when its members are chaged.
        /// </summary>
        Cache,

        /// <summary>
        /// Validate hash code does not change. Requires computing hash function every call.
        /// </summary>
        ImmutableCheck
    }
}
