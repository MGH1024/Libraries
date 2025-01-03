using FluentValidation;

namespace Security.Application.Features.Users.Commands.UpdateFromAuth;

public class UpdateUserFromAuthCommandValidator : AbstractValidator<UpdateUserFromAuthCommand>
{
    public UpdateUserFromAuthCommandValidator()
    {
        RuleFor(c => c.FirstName).NotEmpty().MinimumLength(2);
        RuleFor(c => c.LastName).NotEmpty().MinimumLength(2);
        RuleFor(c => c.Password).NotEmpty().MinimumLength(4);
        RuleFor(c => c.OldPassword).NotEmpty().MinimumLength(4);
        RuleFor(c => c.Password).Equal(c => c.ConfirmPassword);
    }
}
