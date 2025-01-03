namespace Security.Application.Features.OperationClaims.Queries.GetList;

public class GetListOperationClaimListItemDto(int id, string name)
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public GetListOperationClaimListItemDto() : this(0, string.Empty)
    {
    }
}
