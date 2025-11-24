using FluentValidation;

namespace Library.Application.Features.PublicLibraries.Commands.CreateLibrary;

public class CreateLibraryCommandValidator :AbstractValidator<CreateLibraryCommand>
{
    public CreateLibraryCommandValidator()
    {
        RuleFor(a => a.District).IsInEnum();
        RuleFor(a => a.RegistrationTime).NotEmpty();
        RuleFor(a => a.Code).NotEmpty().MaximumLength(3);
        RuleFor(a => a.Name).NotEmpty().MaximumLength(128);
        RuleFor(a => a.Location).NotEmpty().MaximumLength(256);
    }
}