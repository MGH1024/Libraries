using Library.Domain.Libraries.Events;
using Library.Domain.Libraries.Exceptions;
using Library.Domain.Libraries.ValueObjects;
using Library.Domain.Members.ValueObjects;
using MGH.Core.Domain.Base;

public class PublicLibrary : AggregateRoot<Guid>
{
    public Name Name { get; private set; }
    public Code Code { get; private set; }
    public Location Location { get; private set; }
    public District District { get; private set; }
    public RegistrationDate RegistrationDate { get; private set; }

    private readonly List<Staff> _staves = new();
    public IReadOnlyCollection<Staff> LibraryStaves => _staves.AsReadOnly();

    private PublicLibrary() { }

    public static PublicLibrary Create(
        Name name,
        Code code,
        Location location,
        District district,
        RegistrationDate registrationDate)
    {
        var library = new PublicLibrary
        {
            Id = Guid.NewGuid(),
            Name = name,
            Code = code,
            Location = location,
            District = district,
            RegistrationDate = registrationDate
        };

        library.AddDomainEvent(new LibraryAddedDomainEvent(
            name, code, location, district, registrationDate));

        return library;
    }

    public void Update(
        Name name,
        Location location,
        District district,
        RegistrationDate registrationDate)
    {
        Name = name;
        Location = location;
        District = district;
        RegistrationDate = registrationDate;

        AddDomainEvent(new LibraryUpdatedDomainEvent(
            Id, name, location, district, registrationDate));
    }

    public void Remove()
    {
        if (_staves.Any())
            throw new LibraryHasStavesException();

        AddDomainEvent(new LibraryDeletedDomainEvent(Id));
    }

    public void AddStaff(Staff staff)
    {
        if (HasStaffWith(staff.NationalCode))
            throw new LibraryStaffAlreadyExistException();

        _staves.Add(staff);

        AddDomainEvent(new StaffAddedDomainEvent(
            Id, staff.Name, staff.Position, staff.NationalCode));
    }

    public void RemoveStaff(NationalCode nationalCode)
    {
        var staff = _staves.FirstOrDefault(s => s.NationalCode == nationalCode);
        if (staff is null)
            throw new LibraryStaffNotFoundException();

        _staves.Remove(staff);
        AddDomainEvent(new StaffDeletedDomainEvent(Id, nationalCode));
    }

    private bool HasStaffWith(NationalCode nationalCode)
        => _staves.Any(s => s.NationalCode == nationalCode);
}
