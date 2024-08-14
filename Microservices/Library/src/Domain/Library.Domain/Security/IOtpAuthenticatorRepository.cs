using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Security;

public interface IOtpAuthenticatorRepository
{
    Task<OtpAuthenticator> GetAsync(GetModel<OtpAuthenticator> getBaseModel);

    Task<IPaginate<OtpAuthenticator>> GetListAsync(GetListAsyncModel<OtpAuthenticator> getListAsyncModel);

    Task<IPaginate<OtpAuthenticator>> GetDynamicListAsync(
        GetDynamicListAsyncModel<OtpAuthenticator> dynamicListAsyncModel);

    Task<OtpAuthenticator> AddAsync(OtpAuthenticator entity, CancellationToken cancellationToken);
    Task<OtpAuthenticator> DeleteAsync(OtpAuthenticator entity, bool permanent = false);
    Task<OtpAuthenticator> UpdateAsync(OtpAuthenticator entity,CancellationToken  cancellationToken);
}