using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Policies;
using Domain.Entities.Libraries.ValueObjects;

namespace Domain.Entities.Libraries.Factories;

public class LibraryFactory(ILibraryPolicy policy) : ILibraryFactory
{
    public Library Create(string libraryName, string libraryCode, string libraryLocation,
        DateTime libraryRegistrationDate, District libraryDistrict)
    {
        var policyData = new LibraryPolicyData(libraryDistrict);
        var newLibraryName = policy.GenerateName(policyData, libraryName);
        var library = new Library(new LibraryName(newLibraryName),
            new LibraryCode(libraryCode),
            new LibraryLocation(libraryLocation),
            new LibraryDistrict(libraryDistrict),
            new LibraryRegistrationDate(libraryRegistrationDate));
        return library;
    }
}