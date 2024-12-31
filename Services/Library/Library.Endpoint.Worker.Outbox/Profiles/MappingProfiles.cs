using Library.Application.Features.OutBoxes.Commands.UpdateProcessAt;
using Library.Application.Features.OutBoxes.Queries.GetList;
using MGH.Core.Application.Requests;
using MGH.Core.Application.Responses;

namespace Library.Endpoint.Worker.Outbox.Profiles;

public static class MappingProfiles
{
    public static GetOutboxListQuery ToGetOutboxListQuery()
    {
        return new GetOutboxListQuery(new PageRequest
        {
            PageIndex = 0,
            PageSize = 1000
        });
    }

    public static UpdateProcessAtCommand ToUpdateProcessAtCommand(this GetListResponse<GetOutboxListDto> getListResponse)
    {
        return new UpdateProcessAtCommand
        {
            Guids = getListResponse.Items.Select(a => a.Id)
        };
    }
}