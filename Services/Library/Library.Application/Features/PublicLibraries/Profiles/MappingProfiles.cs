using Library.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Application.Responses;
using Library.Domain.Libraries.Events;
using Library.Domain.Libraries.ValueObjects;
using MGH.Core.Infrastructure.Persistence.Paging;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using Library.Application.Features.PublicLibraries.Queries.GetList;
using Library.Application.Features.PublicLibraries.Queries.GetById;
using Library.Application.Features.PublicLibraries.Commands.Update;

namespace Library.Application.Features.PublicLibraries.Profiles;

public static class MappingProfiles
{
    public static GetListResponse<GetListQueryResponse> ToGetListQueryResponse(this IPagedResult<PublicLibrary> libraries)
    {
        return new GetListResponse<GetListQueryResponse>
        {
            Count = libraries.TotalCount,
            Index = libraries.PageIndex,
            Pages = libraries.TotalPages,
            Size = libraries.PageSize,
            HasNext = libraries.HasNext,
            HasPrevious = libraries.HasPrevious,
            Items = libraries.Items.Select(a => new GetListQueryResponse
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Name,
                Location = a.Location,
                CreatedAt = a.CreatedAt,
                District = a.District.Value.ToString(),
            }).ToList()
        };
    }

    public static GetByIdQueryResponse ToGetPublicLibraryByIdResponse(this PublicLibrary publicLibrary)
    {
        if (publicLibrary is null)
            throw new ArgumentNullException(nameof(publicLibrary));

        return new GetByIdQueryResponse
        {
            Id = publicLibrary.Id,
            Code = publicLibrary.Code,
            Title = publicLibrary.Name,
            Location = publicLibrary.Location,
            CreatedAt = publicLibrary.CreatedAt,
            District = publicLibrary.District.Value.ToString(),
        };
    }
}