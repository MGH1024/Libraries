using Security.Application.Features.Users.Queries.GetById;
using Security.Test.Application.Base;

namespace Security.Test.Application.Features.Users.Queries.GetById;

public static class GetUserByIdQueryHandlerFactory
{
   public static GetUserByIdQueryHandler GetUserByIdQueryHandler(HandlerTestsFixture fixture)
   {
      return new GetUserByIdQueryHandler(fixture.MockUnitOfWork.Object,
         fixture.MockMapper.Object, fixture.MockUserBusinessRules.Object);
   }
}