namespace MGH.Core.Infrastructure.Securities.Security.Constants;

public static class GeneralOperationClaims
{
    //Admin
    public const string Admin = "users.admin";

    //User
    public const string GetUsers = "users.get";
    public const string AddUsers = "users.add";
    public const string UpdateUsers = "users.update";
    public const string DeleteUsers = "users.delete";
    
    //Library
    
    public const string GetLibraries = "libraries.get";
    public const string AddLibraries = "libraries.add";
    public const string UpdateLibraries = "libraries.update";
    public const string DeleteLibraries = "libraries.delete";
}
