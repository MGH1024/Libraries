using AutoMapper;
using Moq;
using Security.Application.Features.Auth.Rules;
using Security.Application.Features.Auth.Services;
using Security.Application.Features.Users.Rules;
using Security.Domain;

namespace Security.Test.Application.Base;

public  class HandlerTestsFixture
{
    public Mock<IUow> MockUnitOfWork { get; } = new();
    public Mock<IUserBusinessRules> MockUserBusinessRules { get; } = new();
    public Mock<IMapper> MockMapper { get; } = new();
    public Mock<IAuthBusinessRules> MockAuthBusinessRules { get; } = new();
    public Mock<IAuthService> MockAuthService { get; } = new();
}