using Domain.Entities.Libraries.Constant;
using Domain.Entities.Libraries.Policies;
using Domain.Entities.Libraries.ValueObjects;
using District = Domain.Entities.Libraries.ValueObjects.District;

namespace Domain.Entities.Libraries.Factories;

public class LibraryFactory(ILibraryPolicy policy) : ILibraryFactory
{
    public Library Create(string libraryName, string libraryCode, string libraryLocation,
        DateTime libraryRegistrationDate, Constant.District libraryDistrict)
    {
        var policyData = new LibraryPolicyData(libraryDistrict);
        var newLibraryName = policy.GenerateName(policyData, libraryName);
        
        var library = new Library(new Name(newLibraryName),
            new Code(libraryCode),
            new Location(libraryLocation),
            new District(libraryDistrict),
            new RegistrationDate(libraryRegistrationDate));
        return library;
    }
}