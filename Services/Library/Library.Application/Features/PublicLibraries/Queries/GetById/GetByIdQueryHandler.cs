using MediatR;
using Library.Domain.Libraries;
using Library.Domain.Libraries.Exceptions;

namespace Library.Application.Features.PublicLibraries.Queries.GetById;

public class GetByIdQueryHandler(IPublicLibraryRepository libraryRepository)
    : IRequestHandler<GetByIdQuery, GetByIdQueryResponse>
{
    public async Task<GetByIdQueryResponse> Handle(
        GetByIdQuery request,
        CancellationToken cancellationToken)
    {
        var library = await libraryRepository.GetAsync(request.Id, cancellationToken);
        if (library is null)
            throw new LibraryNotFoundException();

        return new GetByIdQueryResponse
        {
            Id = library.Id,
            Code = library.Code,
            Title = library.Name,
            Location = library.Location,
            CreatedAt = library.CreatedAt,
            District = library.District.Value.ToString(),
        };
    }
}