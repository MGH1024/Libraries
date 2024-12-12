﻿using Domain.Entities.Libraries.Constant;

namespace Domain.Entities.Libraries.Policies;

public class DistrictPolicy : ILibraryPolicy
{
    public string GenerateName(LibraryPolicyData libraryPolicyData, string name)
    {
        var res = libraryPolicyData.DistrictEnum switch
        {
            DistrictEnum.One => "_DistrictOne",
            DistrictEnum.Two => "_DistrictTwo",
            DistrictEnum.Three => "_DistrictThree",
            _ => ""
        };
        return $"{name}{res}";
    }
}