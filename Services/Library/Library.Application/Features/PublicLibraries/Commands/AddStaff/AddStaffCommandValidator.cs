using FluentValidation;

namespace Library.Application.Features.PublicLibraries.Commands.AddStaff;

public class AddStaffCommandValidator :AbstractValidator<AddStaffCommand>
{
    public AddStaffCommandValidator()
    {
        RuleFor(a => a.Name).NotEmpty().MaximumLength(64);
        RuleFor(a => a.NationalCode).NotEmpty().MinimumLength(10);
        RuleFor(a => a.Position).NotEmpty().MaximumLength(64);
        RuleFor(a => a.LibraryId).NotEmpty();
    }
}