using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Policies;
using Domain.Entities.Libraries.ValueObjects;

namespace Domain.Entities.Libraries.Factories;

public class LibraryFactory : ILibraryFactory
{
    private readonly ILibraryPolicy _policy;

    public LibraryFactory(ILibraryPolicy policy)
    {
        _policy = policy;
    }

    public Library Create(string libraryName, string libraryCode, string libraryLocation,
        DateTime libraryRegistrationDate, District libraryDistrict)
    {
        var policyData = new LibraryPolicyData(libraryDistrict);
        var newLibraryName = _policy.GenerateName(policyData, libraryName);

        return new Library(new LibraryName(newLibraryName),
            new LibraryCode(libraryCode),
            new LibraryLocation(libraryLocation),
            new LibraryDistrict(libraryDistrict),
            new LibraryRegistrationDate(libraryRegistrationDate));
    }
}