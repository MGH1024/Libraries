using Domain.Entities.Members.ValueObjects;
using MGH.Core.Domain.BaseEntity;

namespace Domain.Entities.Members;

public class Member :AggregateRoot<Guid>
{
    private FullName _fullName;
    private NationalCode _nationalCode;
    private MobileNumber _mobileNumber;
    private Address _address;

    public Member(FullName fullName, NationalCode nationalCode,
        MobileNumber mobileNumber, Address address)
    {
        Id = Guid.NewGuid();
        _fullName = fullName;
        _nationalCode = nationalCode;
        _mobileNumber = mobileNumber;
        _address = address;
    }
    


    //borrow book
    //return book
    //pay late fee
}