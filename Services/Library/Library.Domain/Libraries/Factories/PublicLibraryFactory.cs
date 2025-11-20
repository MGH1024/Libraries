using Library.Domain.Libraries.Policies;

namespace Library.Domain.Libraries.Factories;

public class PublicLibraryFactory(ILibraryPolicy policy) : IPublicLibraryFactory
{
    public PublicLibrary Create(string name, string code, string location,
        DateTime registrationDate, Constant.DistrictEnum districtEnum)
    {
        var policyData = new LibraryPolicyData(districtEnum);
        var newLibraryName = policy.GenerateName(policyData, name);

        var library = new PublicLibrary(newLibraryName,
            code,
            location,
            districtEnum,
            registrationDate);
        return library;
    }
}