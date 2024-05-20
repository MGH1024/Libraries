namespace MGH.Core.Infrastructure.Mails;

public interface IMailService
{
    void SendMail(Mail mail);
    Task SendEmailAsync(Mail mail);
}
