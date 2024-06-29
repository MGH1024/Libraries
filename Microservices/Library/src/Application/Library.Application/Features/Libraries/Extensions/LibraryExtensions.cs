using Domain.Entities.Libraries.ValueObjects;
using Application.Features.Libraries.Commands.EditLibrary;
using Application.Features.Libraries.Queries.GetList;
using Domain.Entities.Libraries;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;

namespace Application.Features.Libraries.Extensions;

public static class LibraryExtensions
{
    public static GetListResponse<GetLibraryListDto> ToGetLibraryListDto(this IPaginate<Library> libraries)
    {
        return new GetListResponse<GetLibraryListDto>
        {
            Count = libraries.Count,
            Index = libraries.Index,
            Pages = libraries.Pages,
            Size = libraries.Size,
            HasNext = libraries.HasNext,
            HasPrevious = libraries.HasPrevious,
            Items = libraries.Items.Select(a => new GetLibraryListDto
            {
                Id = a.Id,
                Title = a.Name.Value,
                CreatedAt = a.CreatedAt,
            }).ToList()
        };
    }

    public static IEnumerable<Staff> ToStaffList(this List<StaffDto> staffDtOs)
    {
        return staffDtOs.Select(a => a.ToStaff());
    }

    private static Staff ToStaff(this StaffDto staffDto)
    {
        return new Staff(staffDto.Name, staffDto.Position, staffDto.NationalCode);
    }
}