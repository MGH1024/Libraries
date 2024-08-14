using Application.Features.OperationClaims.Rules;
using AutoMapper;
using Domain;
using MediatR;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using Microsoft.EntityFrameworkCore;

namespace Application.Features.OperationClaims.Queries.GetById;

public class GetByIdOperationClaimQuery : IRequest<GetByIdOperationClaimResponse>
{
    public int Id { get; set; }

    public class GetByIdOperationClaimQueryHandler(
        IUow uow,
        IMapper mapper,
        OperationClaimBusinessRules operationClaimBusinessRules)
        : IRequestHandler<GetByIdOperationClaimQuery, GetByIdOperationClaimResponse>
    {
        public async Task<GetByIdOperationClaimResponse> Handle(GetByIdOperationClaimQuery request,
            CancellationToken cancellationToken)
        {
           var operationClaim = await uow.OperationClaim.GetAsync(
                new GetModel<OperationClaim>
                {
                    Predicate = b => b.Id == request.Id,
                    Include = q => q.Include(oc => oc.UserOperationClaims),
                    CancellationToken = cancellationToken
                });
            await operationClaimBusinessRules.OperationClaimShouldExistWhenSelected(operationClaim);
            var response = mapper.Map<GetByIdOperationClaimResponse>(operationClaim);
            return response;
        }
    }
}