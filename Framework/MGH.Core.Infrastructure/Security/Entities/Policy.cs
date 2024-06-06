using MGH.Core.Domain.Entity.Base;

namespace MGH.Core.Infrastructure.Security.Entities;

public class Policy :AuditableEntity<int>
{
    public string Title { get; set; }
    
    public virtual ICollection<PolicyOperationClaim> PolicyOperationClaims { get; set; } = null!;

    public Policy(string title)
    {
        Title = title;
    }

    public Policy(int id,string title) :base(id)
    {
        Title = title;
    }
}