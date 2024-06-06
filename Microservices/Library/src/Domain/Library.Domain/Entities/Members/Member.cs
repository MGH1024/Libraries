using Domain.Entities.Members.ValueObjects;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Members;

public class Member :AggregateRoot<Guid>
{
    private MemberFullName _memberFullName;
    private MemberNationalCode _memberNationalCode;
    private MemberMobileNumber _memberMobileNumber;
    private MemberAddress _memberAddress;

    public Member(MemberFullName memberFullName, MemberNationalCode memberNationalCode,
        MemberMobileNumber memberMobileNumber, MemberAddress memberAddress)
    {
        Id = Guid.NewGuid();
        _memberFullName = memberFullName;
        _memberNationalCode = memberNationalCode;
        _memberMobileNumber = memberMobileNumber;
        _memberAddress = memberAddress;
    }
    


    //borrow book
    //return book
    //pay late fee
}