using Application.Features.Users.Queries.GetById;
using Security.Test.Fixtures;

namespace Security.Test.Builders.Handlers;

public static class GetUserByIdBuilder
{
   public static GetUserByIdQueryHandler GetUserByIdQueryHandlerBuilder(HandlerTestsFixture fixture)
   {
      return new GetUserByIdQueryHandler(fixture.MockUnitOfWork.Object,
         fixture.MockMapper.Object, fixture.MockUserBusinessRules.Object);
   }
}