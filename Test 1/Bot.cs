using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using DSharpPlus;
using DSharpPlus.CommandsNext;
using DSharpPlus.EventArgs;
using DSharpPlus.Interactivity;
using DSharpPlus.Interactivity.Extensions;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Test_1.Commands;
using Microsoft.EntityFrameworkCore.Design;

namespace Test_1
{
    public class Bot
    {
        public DiscordClient Client { get; private set; } //gets the commands but we cant set them
        public InteractivityExtension Interactivity { get; private set; }
        public CommandsNextExtension Commands { get; private set; }
         
        public Bot(IServiceProvider services)
        {
            var json = string.Empty;

            using (var fs = File.OpenRead("config.json"))
            using (var sr = new StreamReader(fs, new UTF8Encoding(false)))
                json = sr.ReadToEnd();

            var configJson = JsonConvert.DeserializeObject<ConfigJson>(json);

            var config = new DiscordConfiguration
            {
                Token = configJson.Token,
                TokenType = TokenType.Bot,
                AutoReconnect = true,
                MinimumLogLevel = LogLevel.Debug,
            };

            Client = new DiscordClient(config);

            Client.Ready += OnClientReady;
            Client.UseInteractivity(new InteractivityConfiguration
            {
                Timeout = TimeSpan.FromSeconds(30)

            });

            var CommandsConfig = new CommandsNextConfiguration
            {
                StringPrefixes = new string[] { configJson.Prefix },
                EnableMentionPrefix = true,
                EnableDms = false,
                CaseSensitive = false,
                DmHelp = true,
                Services = services,
            };

            Commands = Client.UseCommandsNext(CommandsConfig);

            //register commands
            Commands.RegisterCommands<FunCommands>();
            Commands.RegisterCommands<TeamCommands>();
            Commands.RegisterCommands<PollCommands>();
            Commands.RegisterCommands<Moderation>();

            Client.ConnectAsync();

        }
        

        private Task OnClientReady(Object Sender, ReadyEventArgs e)
        {
            return Task.CompletedTask;
        }
    }
}
