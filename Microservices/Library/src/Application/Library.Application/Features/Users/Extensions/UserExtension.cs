using MGH.Core.Infrastructure.Securities.Security.Entities;
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
}