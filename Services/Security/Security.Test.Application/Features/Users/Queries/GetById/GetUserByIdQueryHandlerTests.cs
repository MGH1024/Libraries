using MGH.Core.Infrastructure.Securities.Security.Entities;
using Moq;
using Security.Application.Features.Users.Queries.GetById;
using Security.Test.Application.Base;

namespace Security.Test.Application.Features.Users.Queries.GetById;

public class GetUserByIdQueryHandlerTests(HandlerTestsFixture fixture) : IClassFixture<HandlerTestsFixture>
{
    [Theory]
    [InlineData(1)]
    public async Task Handle_ShouldReturnUserResponse_WhenUserExists(int userId)
    {
        // Arrange
        var handler = GetUserByIdQueryHandlerFactory.GetUserByIdQueryHandler(fixture);
        var userEntity = new User { Id = userId, FirstName = "Test User" };
        var userResponse = new GetUserByIdResponse { Id = userId, FirstName = "Test User" };
        var request = new GetUserByIdQuery { Id = userId };

        fixture.MockUnitOfWork
            .Setup(u => u.User.GetAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);
    
        fixture.MockMapper
            .Setup(m => m.Map<GetUserByIdResponse>(userEntity))
            .Returns(userResponse);

        // Act
        var result = await handler.Handle(request, CancellationToken.None);

        // Assert
        Assert.NotNull(result);
        Assert.Equal(userResponse.Id, result.Id);
        Assert.Equal(userResponse.FirstName, result.FirstName);
    
        fixture.MockUserBusinessRules
            .Verify(r => r.UserShouldBeExistsWhenSelected(userEntity), Times.Once);
    }


    [Theory]
    [InlineData(2)]
    public async Task Handle_ShouldThrowException_WhenUserDoesNotExist(int userId)
    {
        // Arrange
        var handler =GetUserByIdQueryHandlerFactory.GetUserByIdQueryHandler(fixture);
        User? userEntity = null;

        fixture.MockUnitOfWork.Setup(u => u.User.GetAsync(userId, It.IsAny<CancellationToken>()))
            .ReturnsAsync(userEntity);
        fixture.MockUserBusinessRules.Setup(r => r.UserShouldBeExistsWhenSelected(userEntity))
            .ThrowsAsync(new Exception("User not found"));

        var request = new GetUserByIdQuery { Id = userId };

        // Act & Assert
        await Assert.ThrowsAsync<Exception>(() => handler.Handle(request, CancellationToken.None));
        fixture.MockUserBusinessRules.Verify(r => r.UserShouldBeExistsWhenSelected(userEntity), Times.Once);
    }
}