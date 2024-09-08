using Domain.Entities.Members.Exceptions;
using MGH.Core.Domain.Aggregate;

namespace Domain.Entities.Members.ValueObjects;

public class Address : ValueObject
{
    public string Street { get; }
    public string City { get; }
    public string State { get; }
    public string PostalCode { get; }

    public Address(string street, string city, string state, string postalCode)
    {
        Street = street ?? throw new MemberAddressException("street is empty");
        City = city ?? throw new MemberAddressException("city is empty");
        State = state ?? throw new MemberAddressException("state is empty");
        PostalCode = postalCode ?? throw new MemberAddressException("postal code is empty");
    }
    
    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return new object[] { State, City, Street, PostalCode };
    }
}