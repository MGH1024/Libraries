using Application.Features.Users.Constants;
using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MediatR;
using MGH.Core.Application.Pipelines.Authorization;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.Users.Queries.GetById;

[Roles(UsersOperationClaims.GetUsers)]
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
        var user = await uow.User.GetAsync(request.Id,cancellationToken);
        await userBusinessRules.UserShouldBeExistsWhenSelected(user);

        var response = mapper.Map<GetByIdUserResponse>(user);
        return response;
    }
}