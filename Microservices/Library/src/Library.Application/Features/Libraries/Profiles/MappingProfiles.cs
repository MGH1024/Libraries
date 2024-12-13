using Library.Application.Features.Libraries.Commands.AddLibraryStaff;
using Library.Application.Features.Libraries.Commands.EditLibrary;
using Library.Application.Features.Libraries.Commands.RemoveLibrary;
using Library.Application.Features.Libraries.Commands.RemoveLibraryStaff;
using Library.Application.Features.Libraries.Queries.GetList;
using Library.Domain.Entities.Libraries.Events;
using Library.Domain.Entities.Libraries.ValueObjects;
using MGH.Core.Application.Responses;
using MGH.Core.Domain.Entity.Outboxes;
using MGH.Core.Infrastructure.ElasticSearch.ElasticSearch.Models;
using MGH.Core.Infrastructure.Persistence.Models.Filters;
using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.Features.Libraries.Profiles;

public static class MappingProfiles
{
    public static GetListResponse<GetLibraryListDto> ToGetLibraryListDto(this IPaginate<Library.Domain.Entities.Libraries.Library> libraries)
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

    public static GetModel<Library.Domain.Entities.Libraries.Library> ToGetBaseLibraryModel(this string code)
    {
        return new GetModel<Library.Domain.Entities.Libraries.Library>
        {
            Predicate = a => a.Code == code
        };
    }

    public static GetModel<Library.Domain.Entities.Libraries.Library> ToGetBaseLibraryModel(this DeleteLibraryStaffCommand command, CancellationToken cancellationToken)
    {
        return new GetModel<Library.Domain.Entities.Libraries.Library>
        {
            Predicate = a => a.Id == command.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves),
            CancellationToken = cancellationToken
        };
    }

    public static GetModel<Library.Domain.Entities.Libraries.Library> ToGetBaseLibraryModel(this UpdateLibraryCommand request, CancellationToken cancellationToken)
    {
        return new GetModel<Library.Domain.Entities.Libraries.Library>()
        {
            Predicate = a => a.Id == request.LibraryId,
            CancellationToken = cancellationToken
        };
    }

    public static GetModel<Library.Domain.Entities.Libraries.Library> ToGetBaseLibraryModel(this UpdateLibraryWithStavesCommand request, CancellationToken cancellationToken)
    {
        return new GetModel<Library.Domain.Entities.Libraries.Library>
        {
            Predicate = a => a.Id == request.LibraryId, CancellationToken = cancellationToken
        };
    }

    public static GetModel<Library.Domain.Entities.Libraries.Library> ToGetBaseLibraryModel(this DeleteLibraryCommand request, CancellationToken cancellationToken)
    {
        return new GetModel<Library.Domain.Entities.Libraries.Library>
        {
            Predicate = a => a.Id == request.LibraryId,
            CancellationToken = cancellationToken
        };
    }

    public static GetModel<Library.Domain.Entities.Libraries.Library> ToGetBaseLibraryModel(this CreateLibraryStaffCommand request, CancellationToken cancellationToken)
    {
        return new GetModel<Library.Domain.Entities.Libraries.Library>()
        {
            Predicate = a => a.Id == request.LibraryId,
            Include = a => a.Include(b => b.LibraryStaves),
            CancellationToken = cancellationToken,
        };
    }

    public static GetDynamicListModelAsync<Library.Domain.Entities.Libraries.Library> ToGetDynamicListAsyncModel(this GetLibraryListQuery request, CancellationToken cancellationToken)
    {
        var dyn = new DynamicQuery();
        dyn.Filter = new Filter("Name", "contains", "par", "and", null);
        dyn.Sort = null;
        return new GetDynamicListModelAsync<Library.Domain.Entities.Libraries.Library>()
        {
            Dynamic = dyn,
            CancellationToken = cancellationToken
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