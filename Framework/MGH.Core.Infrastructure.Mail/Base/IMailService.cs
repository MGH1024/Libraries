namespace MGH.Core.Infrastructure.Mail.Base;

public interface IMailService
{
    void SendMail(MailKitImplementations.Models.Mail mail);
    Task SendEmailAsync(MailKitImplementations.Models.Mail mail);
}
