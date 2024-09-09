﻿using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;

namespace Domain.Entities.Security;

public interface IEmailAuthenticatorRepository
{
    Task<EmailAuthenticator> GetAsync(GetModel<EmailAuthenticator> getBaseModel);

    Task<IPaginate<EmailAuthenticator>> GetListAsync(GetListAsyncModel<EmailAuthenticator> getListAsyncModel);

    Task<IPaginate<EmailAuthenticator>> GetDynamicListAsync(
        GetDynamicListAsyncModel<EmailAuthenticator> dynamicListAsyncModel);

    Task<EmailAuthenticator> AddAsync(EmailAuthenticator entity, CancellationToken cancellationToken);
    Task<EmailAuthenticator> DeleteAsync(EmailAuthenticator entity, bool permanent = false);
    Task<EmailAuthenticator> UpdateAsync(EmailAuthenticator entity,CancellationToken  cancellationToken);
}