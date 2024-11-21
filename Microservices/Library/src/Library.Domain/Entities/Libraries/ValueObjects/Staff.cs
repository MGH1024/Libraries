using Domain.Entities.Libraries.Exceptions;
using MGH.Core.Domain.BaseEntity;

namespace Domain.Entities.Libraries.ValueObjects;

public class Staff : ValueObject
{
    public Staff(string name, string position, string nationalCode)
    {
        if (string.IsNullOrEmpty(name))
            throw new StaffNameException();
        
        if (string.IsNullOrEmpty(position))
            throw new StaffPositionException();
        
        if (string.IsNullOrWhiteSpace(nationalCode))
            throw new StaffNationalCodeNullException();
        
        if (nationalCode.Length != 10)
            throw new StaffNationalCodeLengthException();
        
        Name = name;
        Position = position;
        NationalCode = nationalCode;
    }

    public string Name { get; }
    public string Position { get; }
    public string NationalCode { get; }

    protected override IEnumerable<object> GetEqualityComponents()
    {
        yield return NationalCode;
    }
}