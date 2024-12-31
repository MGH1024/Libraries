using Domain;
using MediatR;
using AutoMapper;
using Application.Features.Users.Constants;
using Application.Features.Users.Rules;
using MGH.Core.Application.Pipelines.Caching;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Users.Queries.GetById;

[Roles(UsersOperationClaims.GetUsers)]
[Cache(CacheDuration = 5, EntityName = nameof(User))]
public class GetUserByIdQuery : IRequest<GetUserByIdResponse>
{
    public int Id { get; set; }
}

public class GetUserByIdQueryHandler(
    IUow uow,
    IMapper mapper,
    IUserBusinessRules userBusinessRules)
    : IRequestHandler<GetUserByIdQuery, GetUserByIdResponse>
{
    public async Task<GetUserByIdResponse> Handle(GetUserByIdQuery request, CancellationToken cancellationToken)
    {
        var user = await uow.User.GetAsync(request.Id, cancellationToken);
        await userBusinessRules.UserShouldBeExistsWhenSelected(user);

        var response = mapper.Map<GetUserByIdResponse>(user);
        return response;
    }
}