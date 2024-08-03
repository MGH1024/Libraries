namespace MGH.Core.Domain.Entity.Base;

public interface IAuditAble
{
    DateTime CreatedAt { get; set; }

    string CreatedBy { get;set; }

    DateTime? UpdatedAt { get;set; }

    string UpdatedBy { get;set; }

    DateTime? DeletedAt { get;set; }

    string DeletedBy { get;set; }
}

public class AuditAbleEntity<TId>(TId id) : IEntity<TId>, IAuditAble
{
    public AuditAbleEntity() : this(default!)
    {
    }


    public TId Id { get; set; } = id;
    public DateTime CreatedAt { get; set; }
    public string CreatedBy { get; set; }
    public DateTime? UpdatedAt { get; set; }
    public string UpdatedBy { get; set; }
    public DateTime? DeletedAt { get; set; }
    public string DeletedBy { get; set; }
}