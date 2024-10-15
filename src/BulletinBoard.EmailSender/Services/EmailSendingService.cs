using BulletinBoard.Contracts.Emails;
using MassTransit;
using MimeKit;
using SmtpClient = MailKit.Net.Smtp.SmtpClient;

namespace BulletinBoard.EmailSender.Services;

/// <summary>
/// Сервис для рассылки почты.
/// </summary>
public class EmailSendingService : IConsumer<SendEmail>
{
    private const string smtpServer = "smtp.mail.ru";
    private const int port = 465;
    private const bool useSsl = true;
    private const string mailBoxAddress = "noreply-bulletinboard@mail.ru";
    private const string mailBoxName = "noreply-bulletinboard";
    
    //Данные для аутентификации на smtp сервере.
    private const string smtpUserName = "noreply-bulletinboard";
    private const string smtpPassword = "JG83pYnzDXBnzh0HCUVP";
    
    /// <summary>
    /// Получает сообщение из шины.
    /// </summary>
    /// <param name="context">Контекст сообщения.</param>
    public async Task Consume(ConsumeContext<SendEmail> context)
    {
        await SendEmailAsync(context.Message.Receiver, context.Message.Subject, context.Message.Text);
    }

    /// <summary>
    /// Отправляет уведомление на почту.
    /// </summary>
    /// <param name="email">Почта получателя.</param>
    /// <param name="subject">Тема уведомления.</param>
    /// <param name="message">Текст уведомления.</param>
    private async Task SendEmailAsync(string email, string subject, string message)
    {
        //Формировка сообщения
        using var emailMessage = new MimeMessage();
        emailMessage.From.Add(new MailboxAddress(mailBoxName, mailBoxAddress));
        emailMessage.To.Add(new MailboxAddress("", email));
        emailMessage.Subject = subject;
        emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Html)
        {
            Text = message
        };
        
        using (var client = new SmtpClient())
        {
            //Подключение к smtp серверу.
            await client.ConnectAsync(smtpServer, port, useSsl);
            //Аутентификация на сервере.
            await client.AuthenticateAsync(smtpUserName, smtpPassword);
            //Отправлка сообщения.
            await client.SendAsync(emailMessage);
            //Отключение от сервера.
            await client.DisconnectAsync(true);
        }
    }
}