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
        //Получение конфигурации из appsettings.json.
        var builder = new ConfigurationBuilder().AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);
        var configuration = builder.Build();
        
        var host = Host.CreateDefaultBuilder(args).Build();
        
        //Настройка шины.
        var rabbitMqSettings = configuration.GetSection("RabbitMq");
        var busControl = Bus.Factory.CreateUsingRabbitMq(cfg =>
        {
            cfg.Host(rabbitMqSettings.GetValue<string>("Host"), h =>
            {
                h.Username(rabbitMqSettings.GetValue<string>("Username"));
                h.Password(rabbitMqSettings.GetValue<string>("Password"));
            });
            cfg.ReceiveEndpoint("SendEmail", e =>
            {
                e.Consumer<EmailSendingService>();
            });
        });
        
        //Запуск шины.
        await busControl.StartAsync(new CancellationToken());
        
        await host.RunAsync();
    }
}