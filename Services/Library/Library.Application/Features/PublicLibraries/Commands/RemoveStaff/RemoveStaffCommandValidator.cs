using FluentValidation;

namespace Library.Application.Features.PublicLibraries.Commands.RemoveStaff;

public class RemoveStaffCommandValidator : AbstractValidator<RemoveStaffCommand>
{
    public RemoveStaffCommandValidator()
    {
        RuleFor(a => a.LibraryId).NotEmpty();
        RuleFor(a => a.NationalCode).MaximumLength(10).NotEmpty();
    }
}
