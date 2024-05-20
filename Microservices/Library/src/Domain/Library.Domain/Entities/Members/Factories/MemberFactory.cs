using Domain.Entities.Members.ValueObjects;

namespace Domain.Entities.Members.Factories;

public class MemberFactory : IMemberFactory
{
    public Member Create( MemberFullName memberFullName, MemberNationalCode memberNationalCode,
        MemberMobileNumber memberMobileNumber, MemberAddress memberAddress)
    {
        return new Member(memberFullName, memberNationalCode,
            memberMobileNumber, memberAddress);
    }
}