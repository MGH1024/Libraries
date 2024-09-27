using System.Collections;
using System.Linq.Dynamic.Core;
using Domain.Repositories;
using MGH.Core.Domain.Entity.Base;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Base.Repository;
using MGH.Core.Persistence.Extensions;
using MGH.Core.Persistence.Models.Filters.GetModels;
using MGH.Core.Persistence.Models.Paging;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;
using Persistence.Contexts;

namespace Persistence.Repositories.Security;

public class OtpAuthenticatorRepository(SecurityDbContext securityDbContext) :Repository<OtpAuthenticator,int>(securityDbContext),
    IOtpAuthenticatorRepository
{
    public IQueryable<OtpAuthenticator> Query() => securityDbContext.Set<OtpAuthenticator>();
}