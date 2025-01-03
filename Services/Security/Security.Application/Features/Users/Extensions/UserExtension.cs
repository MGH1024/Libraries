using MGH.Core.Infrastructure.Persistence.Models.Filters.GetModels;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using Security.Application.Features.Users.Queries.GetList;

namespace Security.Application.Features.Users.Extensions;

public static class UserExtension
{
    public static GetBaseModel<User> ToGetBaseUser(this int id)
    {
        return new GetBaseModel<User>
        {
            Predicate = u => u.Id == id,
            EnableTracking = false,
        };
    }

    public static GetBaseModel<User> ToGetBaseUser(this string email)
    {
        return new GetBaseModel<User>
        {
            Predicate = u => u.Email == email,
            EnableTracking = false
        };
    }

    public static GetBaseModel<User> ToGetBaseUser(this string email, int id)
    {
        return new GetBaseModel<User>
        {
            Predicate = u => u.Id != id && u.Email == email,
            EnableTracking = false
        };
    }

    public static void SetHashPassword(this User user, HashingHelperModel hashingHelperModel)
    {
        user.PasswordHash = hashingHelperModel.PasswordHash;
        user.PasswordSalt = hashingHelperModel.PasswordSalt;
    }

    public static GetListModelAsync<User> ToGetListAsyncModel(this GetListUserQuery getListUserQuery,
        CancellationToken cancellationToken)
    {
        return new GetListModelAsync<User>
        {
            Index = getListUserQuery.PageRequest.PageIndex,
            Size = getListUserQuery.PageRequest.PageSize,
            CancellationToken = cancellationToken
        };
    }
}