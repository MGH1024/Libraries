using Library.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Application.Responses;
using Library.Domain.Libraries.Events;
using Library.Domain.Libraries.ValueObjects;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using Library.Application.Features.PublicLibraries.Commands.RemoveLibraryStaff;
using Library.Application.Features.PublicLibraries.Commands.AddLibraryStaff;
using Library.Application.Features.PublicLibraries.Commands.RemoveLibrary;
using Library.Application.Features.PublicLibraries.Queries.GetList;
using Library.Application.Features.PublicLibraries.Queries.GetById;
using Library.Application.Features.PublicLibraries.Commands.EditLibrary;

namespace Library.Application.Features.PublicLibraries.Profiles;

public static class MappingProfiles
{
    public static GetListResponse<GetListQueryResponse> ToGetListQueryResponse(this IPaginate<PublicLibrary> libraries)
    {
        return new GetListResponse<GetListQueryResponse>
        {
            Count = libraries.Count,
            Index = libraries.Index,
            Pages = libraries.Pages,
            Size = libraries.Size,
            HasNext = libraries.HasNext,
            HasPrevious = libraries.HasPrevious,
            Items = libraries.Items.Select(a => new GetListQueryResponse
            {
                Id = a.Id,
                Code = a.Code,
                Title = a.Name,
                Location = a.Location,
                CreatedAt = a.CreatedAt,
                District = a.District.Value.ToString(),
            }).ToList()
        };
    }

    public static GetByIdQueryResponse ToGetPublicLibraryByIdResponse(this PublicLibrary publicLibrary)
    {
        if (publicLibrary is null)
            throw new ArgumentNullException(nameof(publicLibrary));

        return new GetByIdQueryResponse
        {
            Id = publicLibrary.Id,
            Code = publicLibrary.Code,
            Title = publicLibrary.Name,
            Location = publicLibrary.Location,
            CreatedAt = publicLibrary.CreatedAt,
            District = publicLibrary.District.Value.ToString(),
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

    public static GetModel<PublicLibrary> ToGetBaseLibraryModel(this DeleteLibraryStaffCommand command)
    {
        return new GetModel<PublicLibrary>
        {
            Predicate = a => a.Id == command.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves),
        };
    }

    public static GetModel<PublicLibrary> ToGetBaseLibraryModel(this UpdateLibraryCommand request)
    {
        return new GetModel<PublicLibrary>()
        {
            Predicate = a => a.Id == request.LibraryId,
        };
    }

    public static GetModel<PublicLibrary> ToGetBaseLibraryModel(this UpdateLibraryWithStavesCommand request)
    {
        return new GetModel<PublicLibrary>
        {
            Predicate = a => a.Id == request.LibraryId
        };
    }

    public static GetModel<PublicLibrary> ToGetBaseLibraryModel(this DeleteLibraryCommand request)
    {
        return new GetModel<PublicLibrary>
        {
            Predicate = a => a.Id == request.LibraryId
        };
    }

    public static GetModel<PublicLibrary> ToGetBaseLibraryModel(this CreateLibraryStaffCommand request)
    {
        return new GetModel<PublicLibrary>()
        {
            Predicate = a => a.Id == request.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves)
        };
    }

    public static GetListModelAsync<PublicLibrary> ToGetListModelAsync(this GetListQuery request)
    {
        return new GetListModelAsync<PublicLibrary>()
        {
            Size = request.PageRequest.PageSize,
            Index = request.PageRequest.PageIndex,
        };
    }

    public static ElasticSearchInsertUpdateModel ToElasticSearchInsertUpdateModel(this LibraryCreatedDomainEvent libraryCreatedDomainEvent)
    {
        return new ElasticSearchInsertUpdateModel(libraryCreatedDomainEvent)
        {
            IndexName = "libraries",
            ElasticId = libraryCreatedDomainEvent.Id
        };
    }

    public static LibraryCreatedDomainEvent ToLibraryCreatedDomainEvent(this PublicLibrary library)
    {
        return new LibraryCreatedDomainEvent(
            library.Name,
            library.Code,
            library.Location,
            library.District,
            library.RegistrationDate);
    }
}