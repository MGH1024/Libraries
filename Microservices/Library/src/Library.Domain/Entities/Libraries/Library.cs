using Library.Domain.Entities.Libraries.Constant;
using Library.Domain.Entities.Libraries.Events;
using Library.Domain.Entities.Libraries.Exceptions;
using Library.Domain.Entities.Libraries.ValueObjects;
using MGH.Core.Domain.BaseEntity;

namespace Library.Domain.Entities.Libraries;

public class Library : AggregateRoot<Guid>
{
    public Name Name { get; private set; }
    public Code Code { get; private set; }
    public Location Location { get; private set; }
    public District District { get; private set; }
    public RegistrationDate RegistrationDate { get; private set; }

    private readonly List<Staff> _staves = new();
    public IReadOnlyCollection<Staff> LibraryStaves => _staves;

    private Library()
    {
    }

    public Library(Name name, Code code, Location location, District district, RegistrationDate registrationDate)
    {
        Id = Guid.NewGuid();
        Name = name;
        Code = code;
        Location = location;
        District = district;
        RegistrationDate = registrationDate;

        AddEvent(new LibraryCreatedDomainEvent(name, code, location, district, registrationDate));
    }

    public void EditLibrary(string name, string libraryCode, string libraryLocation, District libraryDistrict, DateTime libraryRegistrationDate)
    {
        SetLibraryPropertiesForEdit(name, libraryCode, libraryLocation, libraryDistrict, libraryRegistrationDate);
    }

    public void EditLibrary(string name, string libraryCode, string libraryLocation, District libraryDistrict, DateTime libraryRegistrationDate, IEnumerable<Staff> libraryStaves)
    {
        SetLibraryPropertiesForEdit(name, libraryCode, libraryLocation, libraryDistrict, libraryRegistrationDate);
        _staves.RemoveAll(a => !string.IsNullOrEmpty(a.NationalCode));
        _staves.AddRange(libraryStaves);
    }

    public static Task RemoveLibrary(Library library)
    {
        if (library._staves.Count != 0)
            throw new LibraryHasStavesException();
        return Task.CompletedTask;
    }

    public void AddLibraryStaff(Staff staff)
    {
        if (LibraryStaffExist(staff.NationalCode))
            throw new LibraryStaffAlreadyExistException();
        _staves.Add(staff);
    }

    public void RemoveLibraryStaff(string nationalCode)
    {
        var libraryStaff = _staves.FirstOrDefault(a => a.NationalCode.Equals(nationalCode));
        if (libraryStaff is null)
            throw new LibraryStaffNotFoundException();
        _staves.Remove(libraryStaff);
    }

    //BL: you can update only  the name and position of library staff
    public void EditLibraryStaff(Staff staff)
    {
        var oldLibraryStaff = GetLibraryStaffByNationalCode(staff.NationalCode);
        if (oldLibraryStaff is null)
            throw new LibraryStaffNotFoundException();
        RemoveLibraryStaff(oldLibraryStaff.NationalCode);
        AddLibraryStaff(staff);
    }

    private bool LibraryStaffExist(string nationalCode)
        => _staves.Exists(a => a.NationalCode.Equals(nationalCode));


    private void SetLibraryPropertiesForEdit(string name, string code, string location, DistrictEnum districtEnum,
        DateTime registrationDate)
    {
        Name = name;
        Code = code;
        Location = location;
        District = districtEnum;
        RegistrationDate = registrationDate;
    }

    private Staff GetLibraryStaffByNationalCode(string nationalCode)
        => _staves.Find(a => a.NationalCode == nationalCode);
}