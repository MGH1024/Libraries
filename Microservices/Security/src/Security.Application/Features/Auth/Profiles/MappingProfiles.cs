using System.Linq.Expressions;
using Application.Features.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using AutoMapper;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Auth.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<RefreshTkn, RevokedTokenResponse>().ReverseMap();
        CreateMap<LoginCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Email == src.UserForLoginDto.Email)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<LoginCommand, GetModel<User>>>()).ReverseMap();

        CreateMap<User, RegisterCommand>().ReverseMap();

        CreateMap<string, Base<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Email == src)))
            .ForMember(dest => dest.EnableTracking, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.WithDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CancellationToken, opt => opt.Ignore());

        CreateMap<int, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src)))
            .ForMember(dest => dest.EnableTracking, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.WithDeleted, opt => opt.Ignore())
            .ForMember(dest => dest.CancellationToken, opt => opt.Ignore());


        CreateMap<EnableEmailAuthenticatorCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.UserId)))
            .ForMember(dest => dest.CancellationToken, opt => opt.MapFrom<CancellationTokenResolver<EnableEmailAuthenticatorCommand, 
                GetModel<User>>>()).ReverseMap();
    }
}