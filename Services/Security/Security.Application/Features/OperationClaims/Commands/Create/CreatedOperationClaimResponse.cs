using MGH.Core.Application.Responses;

namespace Security.Application.Features.OperationClaims.Commands.Create;

public class CreatedOperationClaimResponse(int id, string name) : IResponse
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public CreatedOperationClaimResponse() : this(0, string.Empty)
    {
    }
}
