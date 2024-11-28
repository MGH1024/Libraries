using System.Linq.Expressions;
using Application.Features.Auth.Commands.Login;
using Application.Features.OperationClaims.Commands.Create;
using Application.Features.OperationClaims.Commands.Delete;
using Application.Features.OperationClaims.Commands.Update;
using Application.Features.OperationClaims.Queries.GetById;
using Application.Features.OperationClaims.Queries.GetList;
using Application.Helpers;
using AutoMapper;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.EF.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.EF.Models.Paging;
using MGH.Core.Infrastructure.Securities.Security.Entities;

namespace Application.Features.OperationClaims.Profiles;

public class MappingProfiles : Profile
{
    public MappingProfiles()
    {
        CreateMap<OperationClaim, CreateOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaim, CreatedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, UpdateOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaim, UpdatedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, DeleteOperationClaimCommand>().ReverseMap();
        CreateMap<OperationClaim, DeletedOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, GetByIdOperationClaimResponse>().ReverseMap();
        CreateMap<OperationClaim, GetListOperationClaimListItemDto>().ReverseMap();
        CreateMap<IPaginate<OperationClaim>, GetListResponse<GetListOperationClaimListItemDto>>().ReverseMap();
        
        CreateMap<DeleteOperationClaimCommand, GetModel<OperationClaim>>()
            .ForMember(dest => dest.Predicate, opt => opt.MapFrom(src => (Expression<Func<OperationClaim, bool>>)(u => u.Id == src.Id)))
            .ForMember(dest => dest.CancellationToken, opt
                => opt.MapFrom<CancellationTokenResolver<DeleteOperationClaimCommand, GetModel<OperationClaim>>>()).ReverseMap();
    }
}
