using AutoMapper;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.RegisterUser;
using Application.Features.Users.Queries.GetList;
using MGH.Core.Application.Requests;

namespace Api.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<LoginCommandDto, LoginCommand>()
            .ForCtorParam("LoginCommandDto", opt => 
                opt.MapFrom(src => src));
        
        CreateMap<RegisterUserCommandDto, RegisterUserCommand>()
            .ForCtorParam("RegisterCommandDto", opt => 
                opt.MapFrom(src => src));

        CreateMap<PageRequest, GetListUserQuery>()
            .ForCtorParam("PageRequest", opt => opt.MapFrom(src => src));
    }
}