using Application.Features.Auth.Rules;
using Application.Features.Auth.Services;
using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using Moq;

namespace Security.Test.Base;

public  class HandlerTestsFixture
{
    public Mock<IUow> MockUnitOfWork { get; } = new();
    public Mock<IUserBusinessRules> MockUserBusinessRules { get; } = new();
    public Mock<IMapper> MockMapper { get; } = new();
    public Mock<IAuthBusinessRules> MockAuthBusinessRules { get; } = new();
    public Mock<IAuthService> MockAuthService { get; } = new();
}