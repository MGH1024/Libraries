using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Exceptions;
using Domain.Entities.Libraries.ValueObjects;
using MGH.Core.Domain.Concretes;

namespace Domain.Entities.Libraries;

public class Library : AggregateRoot<Guid>
{
    public LibraryName LibraryName { get; private set; }
    public LibraryCode LibraryCode { get; private set; }
    public LibraryLocation LibraryLocation { get; private set; }
    public LibraryDistrict LibraryDistrict { get; private set; }
    public LibraryRegistrationDate LibraryRegistrationDate { get; private set; }

    private readonly List<LibraryStaff> _staves = new();
    public IReadOnlyCollection<LibraryStaff> LibraryStaves => _staves;

    private Library()
    {
    }

    public Library(LibraryName libraryName, LibraryCode libraryCode, LibraryLocation libraryLocation,
        LibraryDistrict libraryDistrict, LibraryRegistrationDate libraryRegistrationDate)
    {
        Id = Guid.NewGuid();
        LibraryName = libraryName;
        LibraryCode = libraryCode;
        LibraryLocation = libraryLocation;
        LibraryDistrict = libraryDistrict;
        LibraryRegistrationDate = libraryRegistrationDate;
    }

    public void EditLibrary(string name, string libraryCode, string libraryLocation,
        District libraryDistrict, DateTime libraryRegistrationDate)
    {
        SetLibraryPropertiesForEdit(name, libraryCode, libraryLocation, libraryDistrict, libraryRegistrationDate);
    }

    public void EditLibrary(string name, string libraryCode, string libraryLocation,
        District libraryDistrict, DateTime libraryRegistrationDate,
        IEnumerable<LibraryStaff> libraryStaves)
    {
        SetLibraryPropertiesForEdit(name, libraryCode, libraryLocation, libraryDistrict, libraryRegistrationDate);
        _staves.RemoveAll(a => !string.IsNullOrEmpty(a.NationalCode));
        _staves.AddRange(libraryStaves);
    }

    public Task RemoveLibrary(Library library)
    {
        if (library._staves.Any())
            throw new LibraryHasStavesException();
        return Task.CompletedTask;
    }

    public void AddLibraryStaff(LibraryStaff libraryStaff)
    {
        if (LibraryStaffExist(libraryStaff.NationalCode))
            throw new LibraryStaffAlreadyExistException();
        _staves.Add(libraryStaff);
    }

    public void RemoveLibraryStaff(string nationalCode)
    {
        var libraryStaff = _staves.FirstOrDefault(a => a.NationalCode.Equals(nationalCode));
        if (libraryStaff is null)
            throw new LibraryStaffNotFoundException();
        _staves.Remove(libraryStaff);
    }

    //BL: you can update only  the name and position of library staff
    public void EditLibraryStaff(LibraryStaff libraryStaff)
    {
        var oldLibraryStaff = GetLibraryStaffByNationalCode(libraryStaff.NationalCode);
        if (oldLibraryStaff is null)
            throw new LibraryStaffNotFoundException();
        RemoveLibraryStaff(oldLibraryStaff.NationalCode);
        AddLibraryStaff(libraryStaff);
    }

    private bool LibraryStaffExist(string nationalCode)
        => _staves.Exists(a => a.NationalCode.Equals(nationalCode));


    private void SetLibraryPropertiesForEdit(string name, string libraryCode, string libraryLocation,
        District libraryDistrict, DateTime libraryRegistrationDate)
    {
        LibraryName = new LibraryName(name);
        LibraryCode = new LibraryCode(libraryCode);
        LibraryLocation = new LibraryLocation(libraryLocation);
        LibraryDistrict = new LibraryDistrict(libraryDistrict);
        LibraryRegistrationDate = new LibraryRegistrationDate(libraryRegistrationDate);
    }

    private LibraryStaff GetLibraryStaffByNationalCode(string nationalCode)
        => _staves.Find(a => a.NationalCode == nationalCode);
}