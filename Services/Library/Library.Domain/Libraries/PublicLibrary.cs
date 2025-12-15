using MGH.Core.Domain.Base;
using Library.Domain.Libraries.Events;
using Library.Domain.Libraries.Exceptions;
using Library.Domain.Libraries.ValueObjects;

namespace Library.Domain.Libraries;

public class PublicLibrary : AggregateRoot<Guid>
{
    public Name Name { get; private set; }
    public Code Code { get; private set; }
    public Location Location { get; private set; }
    public District District { get; private set; }
    public RegistrationDate RegistrationDate { get; private set; }

    private readonly List<Staff> _staves = new();
    public IReadOnlyCollection<Staff> LibraryStaves => _staves;

    private PublicLibrary()
    {
    }

    public PublicLibrary(Name name, Code code, Location location, District district, RegistrationDate registrationDate)
    {
        Id = Guid.NewGuid();
        Name = name;
        Code = code;
        Location = location;
        District = district;
        RegistrationDate = registrationDate;

        AddDomainEvent(new LibraryAddedDomainEvent(name, code, location, district, registrationDate));
    }

    public void UpdateLibrary(
        string name,
        string libraryLocation,
        District libraryDistrict,
        DateTime libraryRegistrationDate)
    {
        Name = name;
        Location = libraryLocation;
        District = libraryDistrict;
        RegistrationDate = libraryRegistrationDate;

        AddDomainEvent(new LibraryUpdatedDomainEvent(
            Id,
            name,
            libraryLocation,
            libraryDistrict,
            libraryRegistrationDate));
    }

    public void RemoveLibrary()
    {
        if (_staves.Count != 0)
            throw new LibraryHasStavesException();
        AddDomainEvent(new LibraryDeletedDomainEvent(Id));
    }

    public void AddLibraryStaff(Staff staff)
    {
        if (LibraryStaffExist(staff.NationalCode))
            throw new LibraryStaffAlreadyExistException();
        _staves.Add(staff);
        AddDomainEvent(new StaffAddedDomainEvent(
            Id,
            staff.Name,
            staff.Position,
            staff.NationalCode
            ));
    }

    public void RemoveLibraryStaff(string nationalCode)
    {
        var libraryStaff = _staves.FirstOrDefault(a => a.NationalCode.Equals(nationalCode));
        if (libraryStaff is null)
            throw new LibraryStaffNotFoundException();
        _staves.Remove(libraryStaff);
        AddDomainEvent(new StaffDeletedDomainEvent(Id, nationalCode));
    }

    private bool LibraryStaffExist(string nationalCode)
        => _staves.Exists(a => a.NationalCode.Equals(nationalCode));
}