using MGH.Core.Domain.Entities;
using Microsoft.EntityFrameworkCore;
using MGH.Core.Application.Responses;
using Library.Domain.Libraries.Events;
using Library.Domain.Libraries.ValueObjects;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using MGH.Core.Infrastructure.Persistence.Models.Filters;
using Library.Application.Features.Libraries.Queries.GetList;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using Library.Application.Features.Libraries.Commands.EditLibrary;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using Library.Application.Features.Libraries.Commands.RemoveLibrary;
using Library.Application.Features.Libraries.Commands.AddLibraryStaff;
using Library.Application.Features.Libraries.Commands.RemoveLibraryStaff;

namespace Library.Application.Features.Libraries.Profiles;

public static class MappingProfiles
{
    public static GetListResponse<GetLibraryListDto> ToGetLibraryListDto(this IPaginate<Domain.Libraries.Library> libraries)
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

    public static GetModel<Domain.Libraries.Library> ToGetBaseLibraryModel(this string code)
    {
        return new GetModel<Domain.Libraries.Library>
        {
            Predicate = a => a.Code == code
        };
    }

    public static GetModel<Domain.Libraries.Library> ToGetBaseLibraryModel(this DeleteLibraryStaffCommand command)
    {
        return new GetModel<Domain.Libraries.Library>
        {
            Predicate = a => a.Id == command.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves),
        };
    }

    public static GetModel<Domain.Libraries.Library> ToGetBaseLibraryModel(this UpdateLibraryCommand request)
    {
        return new GetModel<Domain.Libraries.Library>()
        {
            Predicate = a => a.Id == request.LibraryId,
        };
    }

    public static GetModel<Domain.Libraries.Library> ToGetBaseLibraryModel(this UpdateLibraryWithStavesCommand request)
    {
        return new GetModel<Domain.Libraries.Library>
        {
            Predicate = a => a.Id == request.LibraryId
        };
    }

    public static GetModel<Domain.Libraries.Library> ToGetBaseLibraryModel(this DeleteLibraryCommand request)
    {
        return new GetModel<Domain.Libraries.Library>
        {
            Predicate = a => a.Id == request.LibraryId
        };
    }

    public static GetModel<Domain.Libraries.Library> ToGetBaseLibraryModel(this CreateLibraryStaffCommand request)
    {
        return new GetModel<Domain.Libraries.Library>()
        {
            Predicate = a => a.Id == request.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves)
        };
    }

    public static GetDynamicListModelAsync<Domain.Libraries.Library> ToGetDynamicListAsyncModel(this GetLibraryListQuery request)
    {
        var dyn = new DynamicQuery();
        dyn.Filter = new Filter("Name", "contains", "par", "and", null);
        dyn.Sort = null;
        return new GetDynamicListModelAsync<Domain.Libraries.Library>()
        {
            Dynamic = dyn,
        };
    }

    public static OutboxMessage ToOutBox(this LibraryCreatedDomainEvent libraryCreatedDomainEvent)
    {
        return  new OutboxMessage
        {
            Id = Guid.NewGuid(),
            Type = typeof(LibraryCreatedDomainEvent).ToString(),
            Content = System.Text.Json.JsonSerializer.Serialize(libraryCreatedDomainEvent),
        };
    } 
    
    public static ElasticSearchInsertUpdateModel ToElasticSearchInsertUpdateModel(this LibraryCreatedDomainEvent libraryCreatedDomainEvent)
    {
        return  new ElasticSearchInsertUpdateModel(libraryCreatedDomainEvent)
        {
            IndexName = "libraries",
            ElasticId = libraryCreatedDomainEvent.Id
        };
    } 
}