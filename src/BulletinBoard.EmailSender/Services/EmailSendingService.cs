using BulletinBoard.Contracts.Emails;
using MassTransit;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BulletinBoard.EmailSender.Services;

public class EmailSendingService : IConsumer<SendEmail>
{
    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        await SendEmailAsync(context.Message.receiver, context.Message.subject, context.Message.text);
    }

    public async Task SendEmailAsync(string email, string subject, string message)
    {
        using var emailMessage = new MimeMessage();
        
        emailMessage.From.Add(new MailboxAddress("noreply-bulletinboard", "noreply-bulletinboard@mail.ru"));
        emailMessage.To.Add(new MailboxAddress("User", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
        
        using (var client = new SmtpClient())
        {
            await client.ConnectAsync("smtp.mail.ru", 465, true);
            await client.AuthenticateAsync("noreply-bulletinboard", "JG83pYnzDXBnzh0HCUVP");
            await client.SendAsync(emailMessage);
 
            await client.DisconnectAsync(true);
        }
    }
}