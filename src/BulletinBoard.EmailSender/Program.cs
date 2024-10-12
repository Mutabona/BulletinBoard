using BulletinBoard.Contracts.Emails;
using BulletinBoard.EmailSender.Services;
using MassTransit;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;

namespace BulletinBoard.EmailSender;

public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();
        var host = Host.CreateDefaultBuilder(args).Build();
        
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(configuration.GetSection("RabbitMq").GetValue<string>("Host"), h =>
            {
                h.Username(configuration.GetSection("RabbitMq").GetValue<string>("Username"));
                h.Password(configuration.GetSection("RabbitMq").GetValue<string>("Password"));
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