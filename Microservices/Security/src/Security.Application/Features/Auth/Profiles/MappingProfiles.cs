using System.Linq.Expressions;
using Application.Features.Auth.Commands.EnableEmailAuthenticator;
using Application.Features.Auth.Commands.EnableOtpAuthenticator;
using Application.Features.Auth.Commands.Login;
using Application.Features.Auth.Commands.Register;
using Application.Features.Auth.Commands.RevokeToken;
using Application.Features.Auth.Commands.VerifyEmailAuthenticator;
using Application.Features.Auth.Commands.VerifyOtpAuthenticator;
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

        CreateMap<EnableOtpAuthenticatorCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.UserId)))
            .ForMember(dest => dest.CancellationToken, opt => opt.MapFrom<CancellationTokenResolver<EnableOtpAuthenticatorCommand,
                GetModel<User>>>()).ReverseMap();

        CreateMap<EnableOtpAuthenticatorCommand, GetModel<OtpAuthenticator>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.UserId)))
            .ForMember(dest => dest.CancellationToken, opt => opt.MapFrom<CancellationTokenResolver<EnableOtpAuthenticatorCommand,
                GetModel<OtpAuthenticator>>>()).ReverseMap();


        CreateMap<OtpAuthenticator, EnabledOtpAuthenticatorResponse>()
            .ForMember(dest => dest.SecretKey, opt => opt.MapFrom(src => src.SecretKey));

        CreateMap<RefreshTkn, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src => (Expression<Func<User, bool>>)(u => u.Id == src.UserId)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<RefreshTkn, GetModel<User>>>()).ReverseMap();

        CreateMap<VerifyEmailAuthenticatorCommand, GetModel<EmailAuthenticator>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src =>
                    (Expression<Func<EmailAuthenticator, bool>>)(u => u.ActivationKey == src.VerifyEmailAuthenticatorDto.ActivationKey)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<VerifyEmailAuthenticatorCommand, GetModel<EmailAuthenticator>>>()).ReverseMap();
        
        CreateMap<VerifyOtpAuthenticatorCommand, GetModel<OtpAuthenticator>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src =>
                    (Expression<Func<OtpAuthenticator, bool>>)(u => u.UserId == src.UserId)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<VerifyOtpAuthenticatorCommand, GetModel<OtpAuthenticator>>>()).ReverseMap();
        
        CreateMap<VerifyOtpAuthenticatorCommand, GetModel<User>>()
            .ForMember(dest => dest.Predicate, opt
                => opt.MapFrom(src =>
                    (Expression<Func<User, bool>>)(u => u.Id == src.UserId)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<VerifyOtpAuthenticatorCommand, GetModel<User>>>()).ReverseMap();
    }
}