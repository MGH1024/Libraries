using MediatR;
using Library.Domain.Libraries;
using MGH.Core.Application.Responses;
using Library.Application.Features.PublicLibraries.Profiles;

namespace Library.Application.Features.PublicLibraries.Queries.GetList;

public class GetListQueryHandler(
    IPublicLibraryRepository libraryRepository)
    : IRequestHandler<GetListQuery, GetListResponse<GetListQueryResponse>>
{
    public async Task<GetListResponse<GetListQueryResponse>> Handle(
        GetListQuery request,
        CancellationToken cancellationToken)
    {
        var libraries = await libraryRepository.GetListAsync(request.ToGetListModelAsync());
        return libraries.ToGetListQueryResponse();
    }
}