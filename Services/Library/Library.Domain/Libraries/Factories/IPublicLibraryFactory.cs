using Library.Domain.Libraries.Constant;

namespace Library.Domain.Libraries.Factories;

public interface IPublicLibraryFactory
{
    PublicLibrary Create(string libraryName, string libraryCode, string libraryLocation,
        DateTime libraryRegistrationDate,DistrictEnum  libraryDistrictEnum);
}