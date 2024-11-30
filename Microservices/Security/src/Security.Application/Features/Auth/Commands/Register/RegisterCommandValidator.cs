using FluentValidation;

namespace Application.Features.Auth.Commands.Register;

public class RegisterCommandValidator : AbstractValidator<RegisterCommand>
{
    public RegisterCommandValidator()
    {
        RuleFor(c => c.RegisterCommandDto.FirstName).NotEmpty().MinimumLength(2);
        RuleFor(c => c.RegisterCommandDto.LastName).NotEmpty().MinimumLength(2);
        RuleFor(c => c.RegisterCommandDto.Email).NotEmpty().EmailAddress();
        RuleFor(c => c.RegisterCommandDto.Password).NotEmpty().MinimumLength(4);
    }
}
