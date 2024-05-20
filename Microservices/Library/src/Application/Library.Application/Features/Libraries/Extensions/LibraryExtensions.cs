using Domain.Entities.Libraries.ValueObjects;
using Application.Features.Libraries.Commands.EditLibrary;

namespace Application.Features.Libraries.Extensions;

public static class LibraryExtensions
{
    public static IEnumerable<LibraryStaff> ToStaffList(this List<StaffDto> staffDtOs)
    {
        return staffDtOs.Select(a => a.ToStaff());
    }

    private static LibraryStaff ToStaff(this StaffDto staffDto)
    {
        return new LibraryStaff(staffDto.Name, staffDto.Position, staffDto.NationalCode);
    }
}