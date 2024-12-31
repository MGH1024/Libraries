using Library.Domain.Members.ValueObjects;

namespace Library.Domain.Members.Factories;

public interface IMemberFactory
{
    Member Create(FullName fullName, NationalCode nationalCode,
        MobileNumber mobileNumber, Address address);
}