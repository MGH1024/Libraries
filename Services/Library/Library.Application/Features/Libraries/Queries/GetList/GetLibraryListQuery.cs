using MediatR;
using Library.Domain.Libraries;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;
using MGH.Core.Application.Pipelines.Authorization;
using Library.Application.Features.Libraries.Profiles;
using Library.Application.Features.Libraries.Constants;

namespace Library.Application.Features.Libraries.Queries.GetList;

[Roles(LibraryOperationClaims.GetLibraries)]
public class GetLibraryListQuery(PageRequest pageRequest) : IRequest<GetListResponse<GetLibraryListDto>>
{
    public PageRequest PageRequest { get; set; } = pageRequest;

    public GetLibraryListQuery() : this(new PageRequest { PageIndex = 0, PageSize = 10 })
    {
    }
}

public class GetLibraryListQueryHandler(IPublicLibraryRepository libraryRepository)
    : IRequestHandler<GetLibraryListQuery, GetListResponse<GetLibraryListDto>>
{
    public async Task<GetListResponse<GetLibraryListDto>> Handle(GetLibraryListQuery request,
        CancellationToken cancellationToken)
    {
        var libraries = await libraryRepository
            .GetDynamicListAsync(request.ToGetDynamicListAsyncModel());
        return libraries.ToGetLibraryListDto();
    }
}