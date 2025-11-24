using MediatR;
using Library.Domain.Libraries;
using Library.Application.Features.PublicLibraries.Profiles;

namespace Library.Application.Features.PublicLibraries.Queries.GetById;

public class GetByIdQueryHandler(IPublicLibraryRepository libraryRepository) 
    : IRequestHandler<GetByIdQuery, GetByIdQueryResponse>
{
    public async Task<GetByIdQueryResponse> Handle(
        GetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var library = await libraryRepository.GetAsync(request.Id, cancellationToken);
        return library.ToGetPublicLibraryByIdResponse();
    }
}