using MGH.Core.Application.DTOs.Base;

namespace Application.Features.Libraries.Queries.GetList;

public class GetLibraryListDto(Guid id, string title,DateTime createdDate) : IDto
{
    public Guid Id { get; set; } = id;
    public string Title { get; set; } = title;
    public DateTime CreatedAt { get; set; } = createdDate;

    public GetLibraryListDto() : this(new Guid(), string.Empty,new DateTime())
    {
    }
}
