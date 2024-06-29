﻿using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Events;
using Domain.Entities.Libraries.Exceptions;
using Domain.Entities.Libraries.ValueObjects;
using MGH.Core.Domain.Aggregate;
using District = Domain.Entities.Libraries.ValueObjects.District;

namespace Domain.Entities.Libraries;

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

    public Library(Name name, Code code, Location location,
        District district, RegistrationDate registrationDate)
    {
        Id = Guid.NewGuid();
        Name = name;
        Code = code;
        Location = location;
        District = district;
        RegistrationDate = registrationDate;
        
        AddEvent(new LibraryCreatedDomainEvent(name,code,location,
            (int)district.Value,registrationDate));
    }

    public void EditLibrary(string name, string libraryCode, string libraryLocation,
        Constant.District libraryDistrict, DateTime libraryRegistrationDate)
    {
        SetLibraryPropertiesForEdit(name, libraryCode, libraryLocation, libraryDistrict, libraryRegistrationDate);
    }

    public void EditLibrary(string name, string libraryCode, string libraryLocation,
        Constant.District libraryDistrict, DateTime libraryRegistrationDate,
        IEnumerable<Staff> libraryStaves)
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


    private void SetLibraryPropertiesForEdit(string name, string libraryCode, string libraryLocation,
        Constant.District libraryDistrict, DateTime libraryRegistrationDate)
    {
        Name = new Name(name);
        Code = new Code(libraryCode);
        Location = new Location(libraryLocation);
        District = new District(libraryDistrict);
        RegistrationDate = new RegistrationDate(libraryRegistrationDate);
    }

    private Staff GetLibraryStaffByNationalCode(string nationalCode)
        => _staves.Find(a => a.NationalCode == nationalCode);
}