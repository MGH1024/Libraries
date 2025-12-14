using MediatR;
using Library.Domain.Libraries;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Paging;
using MGH.Core.Infrastructure.Persistence.Specifications;

namespace Library.Application.Features.PublicLibraries.Queries.GetList;

public class GetListQueryHandler(
    IPublicLibraryRepository libraryRepository)
    : IRequestHandler<GetListQuery, GetListResponse<GetListQueryResponse>>
{
    public async Task<GetListResponse<GetListQueryResponse>> Handle(
        GetListQuery request,
        CancellationToken cancellationToken)
    {
        var libraries = await libraryRepository
            .GetListAsync(new PagedSpecification<PublicLibrary>()
            {
                PageSize = request.PageRequest.PageSize,
                PageIndex = request.PageRequest.PageIndex,
            });
        return ToGetListQueryResponse(libraries);
    }

    private GetListResponse<GetListQueryResponse> ToGetListQueryResponse(IPagedResult<PublicLibrary> libraries)
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
}