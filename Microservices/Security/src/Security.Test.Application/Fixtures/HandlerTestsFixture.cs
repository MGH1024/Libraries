using Application.Features.Users.Rules;
using AutoMapper;
using Domain;
using Moq;

namespace Security.Test.Fixtures;

public  class HandlerTestsFixture
{
    public Mock<IUow> MockUnitOfWork { get; } = new();
    public Mock<IUserBusinessRules> MockUserBusinessRules { get; } = new();
    public Mock<IMapper> MockMapper { get; } = new();
}