using FluentValidation;

namespace Application.Features.Libraries.Commands.CreateLibrary;

public class CreateLibraryCommandValidator :AbstractValidator<CreateLibraryCommand>
{
    public CreateLibraryCommandValidator()
    {
        RuleFor(a => a.Name).NotEmpty().MaximumLength(128);
        RuleFor(a => a.Code).NotEmpty().MaximumLength(3);
        RuleFor(a => a.Location).NotEmpty().MaximumLength(256);
        RuleFor(a => a.District).IsInEnum();
        RuleFor(a => a.RegistrationDate).NotEmpty();
    }
}