using System.Collections;
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

public class UserRepository(SecurityDbContext securityDbContext) :Repository<User,int>(securityDbContext), IUserRepository
{
    public IQueryable<User> Query() => securityDbContext.Set<User>();
}