using MGH.Core.Application.Responses;

namespace Security.Application.Features.OperationClaims.Queries.GetById;

public class GetByIdOperationClaimResponse(int id, string name) : IResponse
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public GetByIdOperationClaimResponse() : this(0, string.Empty)
    {
    }
}
