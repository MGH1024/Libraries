using Domain.Entities.Members.ValueObjects;

namespace Domain.Entities.Members.Factories;

public interface IMemberFactory
{
    Member Create(MemberFullName memberFullName, MemberNationalCode memberNationalCode,
        MemberMobileNumber memberMobileNumber, MemberAddress memberAddress);
}