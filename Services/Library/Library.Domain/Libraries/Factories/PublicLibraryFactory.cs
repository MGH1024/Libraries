using Library.Domain.Libraries.Constant;
using Library.Domain.Libraries.Factories;
using Library.Domain.Libraries.Policies;
using Library.Domain.Libraries.ValueObjects;

public class PublicLibraryFactory : IPublicLibraryFactory
{
    private readonly ILibraryPolicy _policy;

    public PublicLibraryFactory(ILibraryPolicy policy)
    {
        _policy = policy;
    }

    public PublicLibrary Create(
        string name,
        string code,
        string location,
        DateTime registrationDate,
        DistrictEnum district)
    {
        var policyData = new LibraryPolicyData(district);

        var generatedName = _policy.GenerateName(policyData, name);

        var libraryName = new Name(generatedName);
        var libraryCode = new Code(code);
        var libraryLocation = new Location(location);
        var libraryDistrict = new District(district);
        var libraryRegistrationDate = new RegistrationDate(registrationDate);

        return PublicLibrary.Create(
            libraryName,
            libraryCode,
            libraryLocation,
            libraryDistrict,
            libraryRegistrationDate);
    }

    public void Update(
    PublicLibrary library,
    string name,
    string location,
    District district,
    DateTime registrationDate)
    {
        var updatedName = new Name(name);
        var updatedLocation = new Location(location);
        var updatedDistrict = new District(district);
        var updatedRegistrationDate = new RegistrationDate(registrationDate);

        library.Update(
            updatedName,
            updatedLocation,
            updatedDistrict,
            updatedRegistrationDate);
    }

}
