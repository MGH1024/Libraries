using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Repositories;

public interface IEmailAuthenticatorRepository
{
    Task<EmailAuthenticator> GetAsync(GetModel<EmailAuthenticator> getBaseModel);
    Task<IPaginate<EmailAuthenticator>> GetListAsync(GetListModelAsync<EmailAuthenticator> getListAsyncModel);
    Task<IPaginate<EmailAuthenticator>> GetDynamicListAsync(GetDynamicListModelAsync<EmailAuthenticator> dynamicListAsyncModel);
    Task<EmailAuthenticator> AddAsync(EmailAuthenticator entity, CancellationToken cancellationToken);
    Task<EmailAuthenticator> DeleteAsync(EmailAuthenticator entity, bool permanent = false);
    Task<EmailAuthenticator> UpdateAsync(EmailAuthenticator entity, CancellationToken cancellationToken);
}