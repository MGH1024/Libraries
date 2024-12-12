using Library.Domain.Entities.Members.ValueObjects;

namespace Library.Domain.Entities.Members.Factories;

public interface IMemberFactory
{
    Member Create(FullName fullName, NationalCode nationalCode,
        MobileNumber mobileNumber, Address address);
}