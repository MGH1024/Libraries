namespace Library.Application.Features.PublicLibraries.Queries.GetById;

public record GetByIdQueryResponse(
    Guid Id,
    string Code,
    string Title,
    string Location,
    string District,
    DateTime CreatedAt
);

