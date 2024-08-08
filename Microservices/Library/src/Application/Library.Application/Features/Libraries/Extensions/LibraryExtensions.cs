using Application.Features.Libraries.Commands.AddLibraryStaff;
using Domain.Entities.Libraries.ValueObjects;
using Application.Features.Libraries.Commands.EditLibrary;
using Application.Features.Libraries.Commands.RemoveLibrary;
using Application.Features.Libraries.Commands.RemoveLibraryStaff;
using Application.Features.Libraries.Queries.GetList;
using Domain.Entities.Libraries;
using MGH.Core.Application.Responses;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore;

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
                Title = a.Name,
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

    public static GetBaseModel<Library> ToGetBaseLibraryModel(this string code)
    {
        return new GetBaseModel<Library>
        {
            Predicate = a => a.Code == code
        };
    }

    public static GetBaseModel<Library> ToGetBaseLibraryModel(this DeleteLibraryStaffCommand command,
        CancellationToken cancellationToken)
    {
        return new GetBaseModel<Library>
        {
            Predicate = a => a.Id == command.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves),
            CancellationToken = cancellationToken
        };
    }

    public static GetBaseModel<Library> ToGetBaseLibraryModel(this UpdateLibraryCommand request,
        CancellationToken cancellationToken)
    {
        return new GetBaseModel<Library>()
        {
            Predicate = a => a.Id == request.LibraryId,
            CancellationToken = cancellationToken
        };
    }

    public static GetBaseModel<Library> ToGetBaseLibraryModel(this UpdateLibraryWithStavesCommand request,
        CancellationToken cancellationToken)
    {
        return new GetBaseModel<Library>
        {
            Predicate = a => a.Id == request.LibraryId, CancellationToken = cancellationToken
        };
    }

    public static GetBaseModel<Library> ToGetBaseLibraryModel(this DeleteLibraryCommand request,
        CancellationToken cancellationToken)
    {
        return new GetBaseModel<Library>
        {
            Predicate = a => a.Id == request.LibraryId,
            CancellationToken = cancellationToken
        };
    }

    public static GetBaseModel<Library> ToGetBaseLibraryModel(this CreateLibraryStaffCommand request,
        CancellationToken cancellationToken)
    {
        return new GetBaseModel<Library>()
        {
            Predicate = a => a.Id == request.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves),
            CancellationToken = cancellationToken,
        };
    }

    public static GetDynamicListAsyncModel<Library> ToGetBaseLibraryModel(this GetLibraryListQuery request,
        CancellationToken cancellationToken)
    {
        var dyn = new DynamicQuery();
        dyn.Filter = new Filter("Name", "contains", "par", "and", null);
        dyn.Sort = null;
        return new GetDynamicListAsyncModel<Library>()
        {
            Dynamic = dyn,
            CancellationToken = cancellationToken
        };
    }
}