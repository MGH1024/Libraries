﻿using Domain.Repositories;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Persistence.Base.Repository;
using Microsoft.EntityFrameworkCore;
using Persistence.Contexts;

namespace Persistence.Repositories.Security;

public class UserRepository(SecurityDbContext securityDbContext) :Repository<User,int>(securityDbContext), IUserRepository
{
    public IQueryable<User> Query() => securityDbContext.Set<User>();
    public async Task<User> GetByEmailAsync(string email,CancellationToken cancellationToken)
    {
        return await Query().Where(a=>a.Email == email).FirstOrDefaultAsync(cancellationToken);
    }
}