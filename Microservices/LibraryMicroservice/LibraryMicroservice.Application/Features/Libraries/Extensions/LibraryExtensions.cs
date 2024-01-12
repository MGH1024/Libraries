using Application.Features.Libraries.Commands.EditLibrary;
using Domain.Entities.Libraries.ValueObjects;

namespace Application.Features.Libraries.Extensions;

public static class LibraryExtensions
{
    public static IEnumerable<LibraryStaff> ToStaffList(this List<StaffDto> staffDtos)
    {
        return staffDtos.Select(a => a.ToStaff());
    }

    private static LibraryStaff ToStaff(this StaffDto staffDto)
    {
        return new LibraryStaff(staffDto.Name, staffDto.Position, staffDto.NationalCode);
    }
}