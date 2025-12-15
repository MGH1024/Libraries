using FluentValidation;

namespace Library.Application.Features.PublicLibraries.Commands.Remove;

public class RemoveCommandValidator : AbstractValidator<RemoveCommand>
{
    public RemoveCommandValidator()
    {
        RuleFor(a => a.LibraryId).NotEmpty();
    }
}
