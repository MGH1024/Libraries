﻿using Library.Domain.Members.ValueObjects;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Members;

public class Member :Aggregate<Guid>
{
    public FullName FullName { get;private set; }
    public NationalCode NationalCode{ get;private set; }
    public MobileNumber MobileNumber{ get;private set; }
    public Address Address{ get;private set; }

    public Member(FullName fullName, NationalCode nationalCode,
        MobileNumber mobileNumber, Address address)
    {
        Id = Guid.NewGuid();
        FullName = fullName;
        NationalCode = nationalCode;
        MobileNumber = mobileNumber;
        Address = address;
    }
    


    //borrow book
    //return book
    //pay late fee
}