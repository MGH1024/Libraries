using Library.Domain.Libraries.Constant;

namespace Library.Domain.Libraries.Factories;

public interface IPublicLibraryFactory
{
    PublicLibrary Create(
        string libraryName,
        string libraryCode,
        string libraryLocation,
        DateTime libraryRegistrationTime,
        DistrictEnum libraryDistrict);

    void Update(
        PublicLibrary library,
        string name,
        string location,
        DistrictEnum district,
        DateTime registrationDate);
}