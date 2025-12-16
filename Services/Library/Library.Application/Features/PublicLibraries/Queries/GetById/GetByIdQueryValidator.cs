using FluentValidation;

namespace Library.Application.Features.PublicLibraries.Queries.GetById;

public class GetByIdQueryValidator :AbstractValidator<GetByIdQuery>
{
    public GetByIdQueryValidator()
    {
        RuleFor(a => a.Id).NotEmpty();
    }
}
