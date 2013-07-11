namespace YetAnotherEqualityComparer
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;
    using YetAnotherEqualityComparer.Implementation;

    public class MemberEqualityComparer<T> : IEqualityComparer<T>
    {
        private readonly List<IMemberSetup<T>> memberSetups = new List<IMemberSetup<T>>();

        private readonly GetHashCodeBehavior getHashCodeBehavior;

        private int? cachedHashCode;

        public MemberEqualityComparer()
            : this(GetHashCodeBehavior.Default)
        {
        }

        public MemberEqualityComparer(GetHashCodeBehavior getHashCodeBehavior)
        {
            this.getHashCodeBehavior = getHashCodeBehavior;
        }

        public MemberEqualityComparer<T> Add<TMember>(Func<T, TMember> memberFunc)
        {
            return Add(memberFunc, EqualityComparer<TMember>.Default);
        }

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

        public bool Equals(T x, T y)
        {
            VerifyHaveMembersSetup();

            bool xIsNull = NullTester<T>.IsNull(x);
            bool yIsNull = NullTester<T>.IsNull(y);

            if (xIsNull && yIsNull)
            {
                return true;
            }

            if (xIsNull || yIsNull)
            {
                return false;
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

        public int GetHashCode(T obj)
        {
            VerifyHaveMembersSetup();

            if (NullTester<T>.IsNull(obj))
            {
                return 0;
            }

            if (getHashCodeBehavior == GetHashCodeBehavior.Cache && cachedHashCode.HasValue)
            {
                return cachedHashCode.Value;
            }

            int hashCode = 0;

            for (int i = 0; i < memberSetups.Count; ++i)
            {
                int leftShift = i % 32;
                int rightShift = 32 - leftShift;

                int memberHashCode = memberSetups[i].GetMemberHashCode(obj);

                hashCode = hashCode ^ ((memberHashCode << leftShift) | (memberHashCode >> rightShift));
            }

            if (getHashCodeBehavior == GetHashCodeBehavior.ImmutableCheck
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
