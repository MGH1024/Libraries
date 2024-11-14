using Domain;
using MediatR;
using AutoMapper;
using Application.Features.Users.Rules;
using Application.Features.Users.Constants;
using MGH.Core.Application.Pipelines.Caching;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Users.Queries.GetById;

[Roles(UsersOperationClaims.GetUsers)]
[Cache(CacheDuration = 5, EntityName = nameof(User))]
public class GetByIdUserQuery : IRequest<GetByIdUserResponse>
{
    public int Id { get; set; }
}

public class GetByIdUserQueryHandler(
    IUow uow,
    IMapper mapper,
    UserBusinessRules userBusinessRules)
    : IRequestHandler<GetByIdUserQuery, GetByIdUserResponse>
{
    public async Task<GetByIdUserResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetAsync(request.Id, cancellationToken);
        await userBusinessRules.UserShouldBeExistsWhenSelected(user);

        var response = mapper.Map<GetByIdUserResponse>(user);
        return response;
    }
}