using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using MediatR;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Users.Queries.GetById;

public class GetByIdUserQuery : IRequest<GetByIdUserResponse>
{
    public int Id { get; set; }

    public class GetByIdUserQueryHandler(
        IUow uow,
        IMapper mapper,
        UserBusinessRules userBusinessRules)
        : IRequestHandler<GetByIdUserQuery, GetByIdUserResponse>
    {
        public async Task<GetByIdUserResponse> Handle(GetByIdUserQuery request, CancellationToken cancellationToken)
        {
            var user = await uow.User.GetAsync(
                new GetModel<User>
                {
                    Predicate = b => b.Id == request.Id, CancellationToken = cancellationToken
                });
            await userBusinessRules.UserShouldBeExistsWhenSelected(user);

            var response = mapper.Map<GetByIdUserResponse>(user);
            return response;
        }
    }
}