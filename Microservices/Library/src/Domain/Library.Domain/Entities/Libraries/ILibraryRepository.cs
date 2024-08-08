using System.Linq.Expressions;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Filters;
using MGH.Core.Infrastructure.Persistence.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore.Query;
using MGH.Core.Infrastructure.Persistence.Persistence.Base;

namespace Domain.Entities.Libraries;

public interface ILibraryRepository 
{
    Task<Library> GetAsync(GetBaseModel<Library> getBaseModel);

    Task<IPaginate<Library>> GetListAsync(GetListAsyncModel<Library> getListAsyncModel);

    Task<IPaginate<Library>> GetDynamicListAsync(GetDynamicListAsyncModel<Library> dynamicListAsyncModel);

    Task<Library> AddAsync(Library entity, CancellationToken cancellationToken);
    Task<Library> DeleteAsync(Library entity, bool permanent = false);
}