namespace YetAnotherEqualityComparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using YetAnotherEqualityComparer.Implementation;

    /// <summary>
    /// Provides configuration of members used for equality checks and has code generation.
    /// </summary>
    /// <typeparam name="T">Type to configure upon.</typeparam>
    public class MemberEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly List<IMemberSetup<T>> memberSetups = new List<IMemberSetup<T>>();

        private const int HashBasis = unchecked((int)2166136261);

        private const int HashPrime = 16777619;

        private int? cachedHashCode;

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberEqualityComparer{T}"/> class.
        /// Sets hash code function behavior <see cref="GetHashCodeBehavior"/> to Default.
        /// Don't forget to call Add method to configure members.
        /// </summary>
        public MemberEqualityComparer()
            : this(GetHashCodeBehavior.Default)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MemberEqualityComparer{T}"/> class.
        /// Don't forget to call Add method to configure members.
        /// </summary>
        /// <param name="getHashCodeBehavior">Specific hash function behavior to use.</param>
        public MemberEqualityComparer(GetHashCodeBehavior getHashCodeBehavior)
        {
            this.GetHashCodeBehavior = getHashCodeBehavior;
        }

        /// <summary>
        /// Gets the value of configured hash function behavior.
        /// </summary>
        public GetHashCodeBehavior GetHashCodeBehavior { get; private set; }

        /// <summary>
        /// Adds the member of type to equality check and hash function generation.
        /// This method is using <see cref="EqualityComparer{TMember}.Default"/> for members.
        /// </summary>
        /// <typeparam name="TMember">Member type.</typeparam>
        /// <param name="memberFunc">Accessor function to return member's value.</param>
        /// <returns>The same instance of <see cref="MemberEqualityComparer{T}"/> to allow calls chaining.</returns>
        public MemberEqualityComparer<T> Add<TMember>(Func<T, TMember> memberFunc)
        {
            return Add(memberFunc, EqualityComparer<TMember>.Default);
        }

        /// <summary>
        /// Adds the member of type to equality check and hash function generation.
        /// </summary>
        /// <typeparam name="TMember">Member type.</typeparam>
        /// <param name="memberFunc">Accessor function to return member's value.</param>
        /// <param name="memberEqualityComparer">Specific <see cref="IEqualityComparer{TMember}"/> to use for member comparison.</param>
        /// <returns>The same instance of <see cref="MemberEqualityComparer{T}"/> to allow calls chaining.</returns>
        public MemberEqualityComparer<T> Add<TMember>(
            Func<T, TMember> memberFunc,
            IEqualityComparer<TMember> memberEqualityComparer)
        {
            if (memberFunc == null)
            {
                throw new ArgumentNullException("memberFunc");
            }

            if (memberEqualityComparer == null)
            {
                throw new ArgumentNullException("memberEqualityComparer");
            }

            memberSetups.Add(new MemberSetup<T, TMember>(memberFunc, memberEqualityComparer));

            return this;
        }

        /// <summary>
        /// Compare <paramref name="x"/> with untyped value <paramref name="y"/>.
        /// To be used in Equals override of type <typeparamref name="T"/>.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>
        /// True if <paramref name="x"/> is equal to <paramref name="y"/> with respect to configured members.
        /// </returns>
        public bool Equals(T x, object y)
        {
            if (y == null)
            {
                return TypeTester<T>.IsNull(x);
            }

            return (y is T) && Equals(x, (T)y);
        }

        /// <summary>
        /// Compare <paramref name="x"/> with <paramref name="y"/>.
        /// </summary>
        /// <param name="x">First value.</param>
        /// <param name="y">Second value.</param>
        /// <returns>
        /// True if <paramref name="x"/> is equal to <paramref name="y"/> with respect to configured members.
        /// </returns>
        public bool Equals(T x, T y)
        {
            VerifyHaveMembersSetup();

            bool xIsNull = TypeTester<T>.IsNull(x);
            bool yIsNull = TypeTester<T>.IsNull(y);

            if (xIsNull && yIsNull)
            {
                return true;
            }

            if (xIsNull || yIsNull)
            {
                return false;
            }

            if (TypeTester<T>.ReferenceEquals(x, y))
            {
                return true;
            }

            bool result = false;

            foreach (IMemberSetup<T> memberSetup in memberSetups)
            {
                result = memberSetup.MemberEquals(x, y);

                if (!result)
                {
                    break;
                }
            }

            return result;
        }

        /// <summary>
        /// Computes hash function of instance of type <typeparamref name="T"/> from hash functions of its members.
        /// </summary>
        /// <param name="obj">Instance of <typeparamref name="T"/> to compute hash function on.</param>
        /// <returns>Hash code.</returns>
        public int GetHashCode(T obj)
        {
            VerifyHaveMembersSetup();

            if (TypeTester<T>.IsNull(obj))
            {
                return 0;
            }

            if (GetHashCodeBehavior == GetHashCodeBehavior.Cache && cachedHashCode.HasValue)
            {
                return cachedHashCode.Value;
            }

            int hashCode = HashBasis;

            for (int i = 0; i < memberSetups.Count; ++i)
            {
                int memberHashCode = memberSetups[i].GetMemberHashCode(obj);

                unchecked
                {
                    hashCode = hashCode ^ (memberHashCode & 0xFF);

                    hashCode *= HashPrime;

                    hashCode = hashCode ^ ((memberHashCode >> 8) & 0xFF);

                    hashCode *= HashPrime;

                    hashCode = hashCode ^ ((memberHashCode >> 16) & 0xFF);

                    hashCode *= HashPrime;

                    hashCode = hashCode ^ ((memberHashCode >> 24) & 0xFF);

                    hashCode *= HashPrime;
                }
            }

            if (GetHashCodeBehavior == GetHashCodeBehavior.ImmutableCheck
                && cachedHashCode.HasValue && cachedHashCode.Value != hashCode)
            {
                throw new InvalidOperationException("Hash code value changed");
            }

            cachedHashCode = hashCode;

            return hashCode;
        }

        private void VerifyHaveMembersSetup()
        {
            if (memberSetups.Count == 0)
            {
                throw new InvalidOperationException(
                    "No member setups were configured. Configure it by calling Add() method");
            }
        }
    }
}
