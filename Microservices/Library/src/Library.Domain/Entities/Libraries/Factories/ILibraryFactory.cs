using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.ValueObjects;
using District = Domain.Entities.Libraries.Constant.District;

namespace Domain.Entities.Libraries.Factories;

public interface ILibraryFactory
{
    Library Create(string libraryName, string libraryCode, string libraryLocation,
        DateTime libraryRegistrationDate,District  libraryDistrict);
}