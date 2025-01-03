using MGH.Core.Application.Responses;

namespace Security.Application.Features.UserOperationClaims.Commands.Delete;

public class DeletedUserOperationClaimResponse : IResponse
{
    public int Id { get; set; }
}
