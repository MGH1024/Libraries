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

        return ToGetPublicLibraryByIdResponse(library);
    }

    private GetByIdQueryResponse ToGetPublicLibraryByIdResponse(PublicLibrary publicLibrary)
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