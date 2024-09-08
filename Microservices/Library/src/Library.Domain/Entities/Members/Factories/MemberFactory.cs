using Domain.Entities.Members.ValueObjects;

namespace Domain.Entities.Members.Factories;

public class MemberFactory : IMemberFactory
{
    public Member Create( FullName fullName, NationalCode nationalCode,
        MobileNumber mobileNumber, Address address)
    {
        return new Member(fullName, nationalCode,
            mobileNumber, address);
    }
}