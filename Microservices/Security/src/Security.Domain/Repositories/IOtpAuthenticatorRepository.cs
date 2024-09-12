using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Repositories;

public interface IOtpAuthenticatorRepository
{
    Task<OtpAuthenticator> GetAsync(GetModel<OtpAuthenticator> getBaseModel);

    Task<IPaginate<OtpAuthenticator>> GetListAsync(GetListModelAsync<OtpAuthenticator> getListAsyncModel);

    Task<IPaginate<OtpAuthenticator>> GetDynamicListAsync(
        GetDynamicListModelAsync<OtpAuthenticator> dynamicListAsyncModel);

    Task<OtpAuthenticator> AddAsync(OtpAuthenticator entity, CancellationToken cancellationToken);
    Task<OtpAuthenticator> DeleteAsync(OtpAuthenticator entity, bool permanent = false,CancellationToken cancellationToken=default);
    Task<OtpAuthenticator> UpdateAsync(OtpAuthenticator entity,CancellationToken  cancellationToken);
}