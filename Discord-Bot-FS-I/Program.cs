using Discord_Bot_FS_I.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Serilog;
using LoggerFactory = Discord_Bot_FS_I.Logging.LoggerFactory;

namespace Discord_Bot_FS_I;

internal static class Program
{
    private static async Task Main()
    {
        Log.Logger = LoggerFactory.ForUnknownContext();

        await Host.CreateDefaultBuilder()
            .UseSerilog()
            .UseConsoleLifetime()
            .ConfigureServices(ConfigureServices)
            .RunConsoleAsync();
    }

    private static void ConfigureServices(HostBuilderContext context, IServiceCollection services)
    {
        //TODO read some file - maybe json - for bot secret, mongo db connection string, discord guild id, ...

        services.AddLogging(builder => builder.AddSerilog(LoggerFactory.ForUnknownContext()));

        var config = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("config.json", false)
            .Build();

        services.AddSingleton<IConfiguration>(config)
            .AddSingleton<MongoDBService>();

        services.AddHostedService<DiscordBotService>();
    }
}
