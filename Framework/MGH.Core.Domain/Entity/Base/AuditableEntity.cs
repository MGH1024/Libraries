namespace MGH.Core.Domain.Entity.Base;

public interface IAuditable
{
    DateTime CreatedAt { get; set; }

    string CreatedBy { get;set; }

    DateTime? UpdatedAt { get;set; }

    string UpdatedBy { get;set; }

    DateTime? DeletedAt { get;set; }

    string DeletedBy { get;set; }
}

public class AuditableEntity<TId> : IEntity<TId>, IAuditable
{
    public AuditableEntity()
    {
        Id = default!;
    }

    public AuditableEntity(TId id)
    {
        Id = id;
    }
    
    
    public TId Id { get; set; }
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}