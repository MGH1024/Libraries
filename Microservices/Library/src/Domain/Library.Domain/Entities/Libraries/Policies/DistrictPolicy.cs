using Domain.Entities.Libraries.Constant;

namespace Domain.Entities.Libraries.Policies;

public class DistrictPolicy : ILibraryPolicy
{
    public string GenerateName(LibraryPolicyData libraryPolicyData, string name)
    {
        var res = libraryPolicyData.District switch
        {
            District.One => "_DistrictOne",
            District.Two => "_DistrictTwo",
            District.Three => "_DistrictThree",
            _ => ""
        };
        return $"{name}{res}";
    }
}