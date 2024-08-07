using Domain.Entities.Libraries.Policies;

namespace Domain.Entities.Libraries.Factories;

public class LibraryFactory(ILibraryPolicy policy) : ILibraryFactory
{
    public Library Create(string name, string code, string location,
        DateTime registrationDate, Constant.District district)
    {
        var policyData = new LibraryPolicyData(district);
        var newLibraryName = policy.GenerateName(policyData, name);

        var library = new Library(newLibraryName,
            code,
            location,
            district,
            registrationDate);
        return library;
    }
}