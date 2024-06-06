﻿using MGH.Core.Domain.Entity.Base;

namespace MGH.Core.Infrastructure.Security.Entities;

public class OperationClaim : AuditableEntity<int>
{
    public string Name { get; set; }

    public virtual ICollection<UserOperationClaim> UserOperationClaims { get; set; } = null!;
    public virtual ICollection<PolicyOperationClaim> PolicyOperationClaims { get; set; } = null!;

    public OperationClaim()
    {
        Name = string.Empty;
    }

    public OperationClaim(string name)
    {
        Name = name;
    }

    public OperationClaim(int id, string name)
        : base(id)
    {
        Name = name;
    }
}
