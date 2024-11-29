namespace Security.Infrastructure.Configurations.Base;

public static class DatabaseTableName
{
    public const string Library = "Libraries";
    public const string Staff = "Staves";
    public const string Outbox = "OutBox";
    
    public const string OperationClaim = "OperationClaims";
    public const string RefreshToken = "RefreshTokens";
    public const string User = "Users";
    public const string UserOperationClaims = "UserOperationClaims";
    public const string Policy = "Policies";
    public const string PolicyOperationClaim = "PolicyOperationClaims";
    
    public const string AuditLog = "AuditLogs";
}