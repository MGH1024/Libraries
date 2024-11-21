using Security.Test.Fixtures;
using Application.Features.Users.Queries.GetById;

namespace Security.Test.Factories;

public class HandlerFactories
{
   public static GetUserByIdQueryHandler GetUserByIdQueryHandlerFactory(HandlerTestsFixture fixture)
   {
      return new GetUserByIdQueryHandler(fixture.MockUnitOfWork.Object,
         fixture.MockMapper.Object, fixture.MockUserBusinessRules.Object);
   }
}