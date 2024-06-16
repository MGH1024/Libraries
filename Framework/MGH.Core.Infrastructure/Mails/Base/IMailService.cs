using MGH.Core.Infrastructure.Mails.MailKitImplementations.Models;

namespace MGH.Core.Infrastructure.Mails.Base;

public interface IMailService
{
    void SendMail(Mail mail);
    Task SendEmailAsync(Mail mail);
}
