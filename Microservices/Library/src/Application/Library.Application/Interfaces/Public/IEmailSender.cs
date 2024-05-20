using Application.Models.Email;

namespace Application.Interfaces.Public;

public interface IEmailSender
{
    Task<bool> SendEmail(Email email);
}