using Application.Features.Users.Queries.GetList;
using MGH.Core.Infrastructure.Securities.Security.Entities;
using MGH.Core.Infrastructure.Securities.Security.Hashing;
using MGH.Core.Persistence.Models.Filters.GetModels;

namespace Application.Features.Users.Extensions;

public static class UserExtension
{
    public static Base<User> ToGetBaseUser(this int id)
    {
        return new Base<User>
        {
            Predicate = u => u.Id == id,
            EnableTracking = false,
        };
    }

    public static Base<User> ToGetBaseUser(this string email)
    {
        return new Base<User>
        {
            Predicate = u => u.Email == email,
            EnableTracking = false
        };
    }

    public static Base<User> ToGetBaseUser(this string email, int id)
    {
        return new Base<User>
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

    public static GetListAsyncModel<User> ToGetListAsyncModel(this GetListUserQuery getListUserQuery,
        CancellationToken cancellationToken)
    {
        return new GetListAsyncModel<User>
        {
            Index = getListUserQuery.PageRequest.PageIndex,
            Size = getListUserQuery.PageRequest.PageSize,
            CancellationToken = cancellationToken
        };
    }
}