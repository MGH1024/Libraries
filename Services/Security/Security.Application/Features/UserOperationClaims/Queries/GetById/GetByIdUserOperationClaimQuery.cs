using AutoMapper;
using MediatR;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.UserOperationClaims.Rules;
using Security.Domain;

namespace Security.Application.Features.UserOperationClaims.Queries.GetById;

public class GetByIdUserOperationClaimQuery : IRequest<GetByIdUserOperationClaimResponse>
{
    public int Id { get; set; }

    public class GetByIdUserOperationClaimQueryHandler(
        IUow uow,
        IMapper mapper,
        IUserOperationClaimBusinessRules userOperationClaimBusinessRules)
        : IRequestHandler<GetByIdUserOperationClaimQuery, GetByIdUserOperationClaimResponse>
    {
        public async Task<GetByIdUserOperationClaimResponse> Handle(
            GetByIdUserOperationClaimQuery request,
            CancellationToken cancellationToken
        )
        {
            var userOperationClaim = await uow.UserOperationClaim.GetAsync(
                new GetModel<UserOperationClaim>
                {
                    Predicate = b => b.Id == request.Id,
                    CancellationToken = cancellationToken
                });
            await userOperationClaimBusinessRules.UserOperationClaimShouldExistWhenSelected(userOperationClaim);

            var userOperationClaimDto =
                mapper.Map<GetByIdUserOperationClaimResponse>(userOperationClaim);
            return userOperationClaimDto;
        }
    }
}