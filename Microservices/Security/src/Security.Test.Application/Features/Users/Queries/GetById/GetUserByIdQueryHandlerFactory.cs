using Application.Features.Users.Queries.GetById;
using Security.Test.Base;

namespace Security.Test.Features.Users.Queries.GetById;

public static class GetUserByIdBuilder
{
   public static GetUserByIdQueryHandler GetUserByIdQueryHandlerBuilder(HandlerTestsFixture fixture)
   {
      return new GetUserByIdQueryHandler(fixture.MockUnitOfWork.Object,
         fixture.MockMapper.Object, fixture.MockUserBusinessRules.Object);
   }
}