namespace YetAnotherEqualityComparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public enum GetHashCodeBehavior
    {
        /// <summary>
        /// Recompute hash function each call
        /// </summary>
        Default = 0,

        /// <summary>
        /// Use cached hash code value in subsequent calls
        /// </summary>
        Cache,

        /// <summary>
        /// Validate hash code does not change every call
        /// </summary>
        ImmutableCheck
    }
}
