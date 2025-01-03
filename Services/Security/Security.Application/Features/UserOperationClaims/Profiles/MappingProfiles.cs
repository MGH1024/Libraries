using System.Linq.Expressions;
using AutoMapper;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Security.Application.Features.UserOperationClaims.Commands.Create;
using Security.Application.Features.UserOperationClaims.Commands.Delete;
using Security.Application.Features.UserOperationClaims.Commands.Update;
using Security.Application.Features.UserOperationClaims.Queries.GetById;
using Security.Application.Features.UserOperationClaims.Queries.GetList;
using Security.Application.Helpers;

namespace Security.Application.Features.UserOperationClaims.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<UserOperationClaim, CreateUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, CreatedUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, UpdateUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, UpdatedUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, DeleteUserOperationClaimCommand>().ReverseMap();
        CreateMap<UserOperationClaim, DeletedUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, GetByIdUserOperationClaimResponse>().ReverseMap();
        CreateMap<UserOperationClaim, GetListUserOperationClaimListItemDto>().ReverseMap();
        CreateMap<IPaginate<UserOperationClaim>, GetListResponse<GetListUserOperationClaimListItemDto>>().ReverseMap();

        CreateMap<DeleteUserOperationClaimCommand, GetModel<UserOperationClaim>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<UserOperationClaim, bool>>)(u => u.Id == src.Id)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<DeleteUserOperationClaimCommand, GetModel<UserOperationClaim>>>()).ReverseMap();

        CreateMap<UpdateUserOperationClaimCommand, GetModel<UserOperationClaim>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<UserOperationClaim, bool>>)(u => u.Id == src.Id)))
            .ForMember(dest => dest.EnableTracking, opt => opt.MapFrom(src => false))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<UpdateUserOperationClaimCommand, GetModel<UserOperationClaim>>>()).ReverseMap();
    }
}