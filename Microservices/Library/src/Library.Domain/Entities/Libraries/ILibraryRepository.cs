using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Entities.Libraries;

public interface ILibraryRepository 
{
    Task<Library> GetAsync(GetModel<Library> getBaseModel);

    Task<IPaginate<Library>> GetListAsync(GetListAsyncModel<Library> getListAsyncModel);

    Task<IPaginate<Library>> GetDynamicListAsync(GetDynamicListAsyncModel<Library> dynamicListAsyncModel);

    Task<Library> AddAsync(Library entity, CancellationToken cancellationToken);
    Task<Library> DeleteAsync(Library entity, bool permanent = false);
}