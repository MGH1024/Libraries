using System.Text.Json;
using Library.Domain.Members.Exceptions;
using MGH.Core.Domain.BaseModels;

namespace Library.Domain.Members.ValueObjects;

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
        yield return Street;
        yield return City;
        yield return State;
        yield return PostalCode;
    }

    // Serialization helper to convert Address to JSON string
    public string Value => JsonSerializer.Serialize(this);

    // Factory method for creating Address from JSON string
    public static Address FromValue(string json)
    {
        return JsonSerializer.Deserialize<Address>(json) ?? throw new MemberAddressException("Invalid address JSON.");
    }
}