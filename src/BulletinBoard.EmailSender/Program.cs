using BulletinBoard.Contracts.Emails;
using BulletinBoard.EmailSender.Services;
using MassTransit;
using Microsoft.Extensions.Hosting;

namespace BulletinBoard.EmailSender;

public class Program
{
    public static async Task Main(string[] args)
    {
        var host = Host.CreateDefaultBuilder(args).Build();
        
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host("rabbit", h =>
            {
                h.Username("guest"); 
                h.Password("guest");
            });
            cfg.ReceiveEndpoint("SendEmail", e =>
            {
                e.Consumer<EmailSendingService>();
            });
        });
        
        await busControl.StartAsync(new CancellationToken());
        
        await host.RunAsync();
    }
}