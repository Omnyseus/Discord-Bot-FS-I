using DSharpPlus;
using DSharpPlus.Entities;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using DSharpPlus.SlashCommands;
using DSharpPlus.SlashCommands.EventArgs;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using LoggerFactory = Discord_Bot_FS_I.Logging.LoggerFactory;
using MLoggerFactory = Microsoft.Extensions.Logging.LoggerFactory;

namespace Discord_Bot_FS_I.Services;

public class DiscordBotService : IHostedService
{
    #region Fields

    private readonly ILogger<DiscordBotService> _logger;
    private readonly DiscordClient _client;

    #endregion

    #region Constructors

    public DiscordBotService(IServiceProvider serviceProvider, ILogger<DiscordBotService> logger, IConfiguration config)
    {
        _logger = logger;

        _client = new DiscordClient(new DiscordConfiguration
        {
            Token = config["Token"] ?? throw new InvalidOperationException("Discord Bot Token is missing in the config.json"),
            TokenType = TokenType.Bot,
            Intents = DiscordIntents.AllUnprivileged | DiscordIntents.MessageContents,
            LoggerFactory = new MLoggerFactory().AddSerilog(LoggerFactory.ForUnknownContext()),
            MinimumLogLevel = LogLevel.Warning,
            LogUnknownEvents = false
        });
        AddInteractivity();
        RegisterCommands(serviceProvider);
    }

    #endregion

    #region Private Methods

    private void RegisterCommands(IServiceProvider serviceProvider)
    {
        var slash = _client.UseSlashCommands(new SlashCommandsConfiguration { Services = serviceProvider });

        slash.SlashCommandErrored += OnSlashCommandErrored;
    }

    private async Task OnSlashCommandErrored(SlashCommandsExtension sender, SlashCommandErrorEventArgs args)
    {
#if DEBUG
        await args.Context.CreateResponseAsync(new DiscordEmbedBuilder()
                .WithTitle(args.Exception.GetType().Name)
                .WithColor(DiscordColor.DarkRed)
                .WithDescription(args.Exception.StackTrace ?? "No StackTrace")
                .AddField("Message", args.Exception.Message)
            , true);
#endif
    }

    private void AddInteractivity()
    {
        _client.UseInteractivity(new InteractivityConfiguration
        {
            //TODO Response Behavior
        });
        _client.ComponentInteractionCreated += OnComponentInteractionCreated;
    }

    private async Task OnComponentInteractionCreated(DiscordClient sender, ComponentInteractionCreateEventArgs args)
    {
#if DEBUG
        await args.Interaction.Channel.SendMessageAsync(builder => builder.WithContent($"Received interaction from {args.Interaction.User.Username} on component with id ${args.Id}"));
#endif
    }

    #endregion

    #region Public Methods

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Connecting Discord Bot...");
        await _client.ConnectAsync();
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Disconnecting Discord Bot...");
        await _client.DisconnectAsync();
    }

    #endregion
}
