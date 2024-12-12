using Domain.Entities.Libraries.Policies;

namespace Domain.Entities.Libraries.Factories;

public class LibraryFactory(ILibraryPolicy policy) : ILibraryFactory
{
    public Library Create(string name, string code, string location,
        DateTime registrationDate, Constant.DistrictEnum districtEnum)
    {
        var policyData = new LibraryPolicyData(districtEnum);
        var newLibraryName = policy.GenerateName(policyData, name);

        var library = new Library(newLibraryName,
            code,
            location,
            districtEnum,
            registrationDate);
        return library;
    }
}