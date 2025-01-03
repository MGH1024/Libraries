using MGH.Core.Application.Responses;

namespace Security.Application.Features.OperationClaims.Commands.Update;

public class UpdatedOperationClaimResponse(int id, string name) : IResponse
{
    public int Id { get; set; } = id;
    public string Name { get; set; } = name;

    public UpdatedOperationClaimResponse() : this(0, string.Empty)
    {
    }
}
