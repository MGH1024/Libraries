using AutoMapper;
using Application.Features.Auth.Commands.Login;
using MGH.Core.Application.DTOs.Security;

namespace Api.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserForLoginDto, LoginCommand>()
            .ForCtorParam("userForLoginDto", opt => opt.MapFrom(src => src))
            .ForCtorParam("ipAddress", opt => opt.MapFrom((src,context) => context.Items["IpAddress"]));
    }
}