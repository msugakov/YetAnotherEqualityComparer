namespace YetAnotherEqualityComparer.Implementation
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Text;

    public interface IMemberSetup<in TObject>
    {
        bool MemberEquals(TObject a, TObject b);

        int GetMemberHashCode(TObject @object);
    }

    public class MemberSetup<TObject, TMember> : IMemberSetup<TObject>
    {
        private readonly Func<TObject, TMember> memberFunc;

        private readonly IEqualityComparer<TMember> memberEqualityComparer;

        public MemberSetup(
            Func<TObject, TMember> memberFunc,
            IEqualityComparer<TMember> memberEqualityComparer)
        {
            this.memberFunc = memberFunc;
            this.memberEqualityComparer = memberEqualityComparer;
        }

        public bool MemberEquals(TObject a, TObject b)
        {
            TMember memberA = memberFunc(a);
            TMember memberB = memberFunc(b);

            return memberEqualityComparer.Equals(memberA, memberB);
        }

        public int GetMemberHashCode(TObject @object)
        {
            TMember member = memberFunc(@object);

            return memberEqualityComparer.GetHashCode(member);
        }
    }
}
