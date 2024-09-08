namespace Domain.Entities.Libraries.Policies;

public interface ILibraryPolicy
{
    string GenerateName(LibraryPolicyData libraryPolicyData,string name);
}