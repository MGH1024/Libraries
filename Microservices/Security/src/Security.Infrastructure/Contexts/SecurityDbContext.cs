﻿using MGH.Core.Infrastructure.Persistence.EF.Interceptors;
using MGH.Core.Infrastructure.Public;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Persistence.Contexts;

public class SecurityDbContext(
    DbContextOptions<SecurityDbContext> options,
    IDateTime dateTime,
    IHttpContextAccessor httpContextAccessor)
    : DbContext(options)
{
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(SecurityDbContext).Assembly);
        base.OnModelCreating(modelBuilder);
    }

    protected override void ConfigureConventions(ModelConfigurationBuilder configurationBuilder)
    {
        configurationBuilder.Properties<string>().HaveMaxLength(128);
        base.ConfigureConventions(configurationBuilder);
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        var currentUserName = httpContextAccessor.HttpContext?.User.Identity?.Name;
        optionsBuilder.AddInterceptors(new AuditFieldsInterceptor(dateTime, currentUserName));
    }

    public DbSet<OperationClaim> OperationClaims { get; set; }
    public DbSet<RefreshTkn> RefreshTokens { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<UserOperationClaim> UserOperationClaims { get; set; }
}