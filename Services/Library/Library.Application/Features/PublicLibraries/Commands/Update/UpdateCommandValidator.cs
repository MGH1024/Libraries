using FluentValidation;

namespace Library.Application.Features.PublicLibraries.Commands.Update;

public class UpdateCommandValidator : AbstractValidator<UpdateCommand>
{
    public UpdateCommandValidator()
    {
        RuleFor(a => a.District).IsInEnum();
        RuleFor(a => a.RegistrationTime).NotEmpty();
        RuleFor(a => a.Name).NotEmpty().MaximumLength(128);
        RuleFor(a => a.Location).NotEmpty().MaximumLength(256);
    }
}