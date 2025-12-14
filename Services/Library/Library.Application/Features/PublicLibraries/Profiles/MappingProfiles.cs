using Library.Domain.Libraries;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Application.Responses;
using Library.Domain.Libraries.Events;
using Library.Domain.Libraries.ValueObjects;
using MGH.Core.Infrastructure.Persistence.Paging;
using MGH.Core.Infrastructure.Persistence.Specifications;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using Library.Application.Features.PublicLibraries.Queries.GetList;
using Library.Application.Features.PublicLibraries.Queries.GetById;
using Library.Application.Features.PublicLibraries.Commands.Remove;
using Library.Application.Features.PublicLibraries.Commands.Update;
using Library.Application.Features.PublicLibraries.Commands.AddStaff;
using Library.Application.Features.PublicLibraries.Commands.RemoveStaff;

namespace Library.Application.Features.PublicLibraries.Profiles;

public static class MappingProfiles
{
    public static GetListResponse<GetListQueryResponse> ToGetListQueryResponse(this IPagedResult<PublicLibrary> libraries)
    {
        return new GetListResponse<GetListQueryResponse>
        {
            Count = libraries.TotalCount,
            Index = libraries.PageIndex,
            Pages = libraries.TotalPages,
            Size = libraries.PageSize,
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

    public static Specification<PublicLibrary> ToGetBaseLibraryModel(
     this RemoveStaffCommand command)
    {
        var spec = new Specification<PublicLibrary>
        {
            Criteria = library => library.Id == command.LibraryId
        };
        spec.Includes.Add(library => library.LibraryStaves);
        return spec;
    }


    public static Specification<PublicLibrary> ToGetBaseLibraryModel(this UpdateCommand request)
    {
        return new Specification<PublicLibrary>()
        {
            Criteria = a => a.Id == request.LibraryId,
        };
    }

    public static Specification<PublicLibrary> ToGetBaseLibraryModel(this UpdateLibraryWithStavesCommand request)
    {
        return new Specification<PublicLibrary>
        {
            Criteria = a => a.Id == request.LibraryId
        };
    }

    public static Specification<PublicLibrary> ToGetBaseLibraryModel(this RemoveCommand request)
    {
        return new Specification<PublicLibrary>
        {
            Criteria = a => a.Id == request.LibraryId
        };
    }

    public static Specification<PublicLibrary> ToGetBaseLibraryModel(this AddStaffCommand request)
    {
        var spec = new Specification<PublicLibrary>()
        {
            Criteria = a => a.Id == request.LibraryId,
        };
        spec.Includes.Add(a => a.LibraryStaves);
        return spec;
    }

    public static PagedSpecification<PublicLibrary> ToGetListModelAsync(this GetListQuery request)
    {
        return new PagedSpecification<PublicLibrary>()
        {
            PageSize = request.PageRequest.PageSize,
            PageIndex = request.PageRequest.PageIndex,
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