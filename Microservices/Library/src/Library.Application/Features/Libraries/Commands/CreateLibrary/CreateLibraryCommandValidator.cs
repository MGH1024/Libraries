using FluentValidation;

namespace Library.Application.Features.Libraries.Commands.CreateLibrary;

public class CreateLibraryCommandValidator :AbstractValidator<CreateLibraryCommand>
{
    public CreateLibraryCommandValidator()
    {
        RuleFor(a => a.Name).NotEmpty().MaximumLength(128);
        RuleFor(a => a.Code).NotEmpty().MaximumLength(3);
        RuleFor(a => a.Location).NotEmpty().MaximumLength(256);
        RuleFor(a => a.DistrictEnum).IsInEnum();
        RuleFor(a => a.RegistrationDate).NotEmpty();
    }
}