using Domain.Entities.Members.ValueObjects;

namespace Domain.Entities.Members.Factories;

public interface IMemberFactory
{
    Member Create(FullName fullName, NationalCode nationalCode,
        MobileNumber mobileNumber, Address address);
}