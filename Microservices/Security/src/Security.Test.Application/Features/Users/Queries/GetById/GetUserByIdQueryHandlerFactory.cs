using Application.Features.Users.Queries.GetById;
using Security.Test.Base;

namespace Security.Test.Features.Users.Queries.GetById;

public static class GetUserByIdQueryHandlerFactory
{
   public static GetUserByIdQueryHandler GetUserByIdQueryHandler(HandlerTestsFixture fixture)
   {
      return new GetUserByIdQueryHandler(fixture.MockUnitOfWork.Object,
         fixture.MockMapper.Object, fixture.MockUserBusinessRules.Object);
   }
}