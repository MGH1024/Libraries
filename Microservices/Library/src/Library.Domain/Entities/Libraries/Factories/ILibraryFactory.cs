using Library.Domain.Entities.Libraries.Constant;

namespace Library.Domain.Entities.Libraries.Factories;

public interface ILibraryFactory
{
    Library Create(string libraryName, string libraryCode, string libraryLocation,
        DateTime libraryRegistrationDate,DistrictEnum  libraryDistrictEnum);
}