using MGH.Core.Application.DTOs.Base;

namespace Application.Features.Libraries.Commands.EditLibrary;

public record StaffDto : IDto
{
    public string Name { get; set; }
    public string Position { get; set; }
    public string NationalCode { get; set; }
}