using MGH.Core.Domain.Entity.Base;

namespace MGH.Core.Infrastructure.Securities.Security.Entities;

public class RefreshTkn : AuditAbleEntity<int>
{
    public int UserId { get; set; }
    public string Token { get; set; }
    public DateTime Expires { get; set; }
    public string CreatedByIp { get; set; }
    public DateTime? Revoked { get; set; }
    public string RevokedByIp { get; set; }
    public string ReplacedByToken { get; set; }

    public string ReasonRevoked { get; set; }

    public virtual User User { get; set; } = null!;

    public RefreshTkn()
    {
        Token = string.Empty;
        CreatedByIp = string.Empty;
    }

    public RefreshTkn(int userId, string token, DateTime expires, string createdByIp)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        CreatedByIp = createdByIp;
    }

    public RefreshTkn(int id, int userId, string token, DateTime expires, string createdByIp)
        : base(id)
    {
        UserId = userId;
        Token = token;
        Expires = expires;
        CreatedByIp = createdByIp;
    }
}
