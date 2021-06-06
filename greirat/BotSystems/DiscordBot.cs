using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace greirat
{
    public class DiscordBot
    {
        private const string ENVIRONMENT_VARIABLE_WITH_BOT_TOKEN = "FOOD_BOT_TOKEN";
        private const char BOT_COMMAND_PREFIX = '!';

    #if DEBUG
        public bool BypassIsFromBotCheck { get; set; }
        public DiscordSocketClient BotClient
        {
            get
            {
                return Client;
            }
        }

    #endif
        
        private DiscordSocketClient Client { get; set; } = new();
        private CommandService Commands { get; set; } = new();
        private BotConfig Config { get; set; } = new();

        #if DEBUG
            public async Task SendMessageToChat ()
            {
                await Client.GetGuild(Config.CurrentConfigData.GuildIDForTests).GetTextChannel(Config.CurrentConfigData.ChannelIDForTests).SendMessageAsync("Test");
            }
        #endif
        
        public async Task Initialize ()
        {
            SubscribeOnClientEvents();
            await GetCommandsModule();
            await StartBot();
        }
        
        private void SubscribeOnClientEvents ()
        {
            Client.Log += Log;
            Client.MessageReceived += ProceedReceivedMessage;
        }

        private Task Log (LogMessage msg)
        {
            Console.WriteLine(msg.ToString());

            return Task.CompletedTask;
        }

        private async Task ProceedReceivedMessage (SocketMessage messageParam)
        {
            if (messageParam is not SocketUserMessage message)
            {
                return;
            }

            if (CheckIfMessageFromBot(message) == true)
            {
                return;
            }

            if (CheckIfTheMessageContainsCommand(message, out int commandPrefixPosition) == false)
            {
                return;
            }

            SocketCommandContext context = new(Client, message);
            await Commands.ExecuteAsync(context, commandPrefixPosition, null);
        }

        private bool CheckIfMessageFromBot (SocketUserMessage message)
        {
        #if DEBUG
            if (BypassIsFromBotCheck == true)
            {
                return false;
            }
        #endif

            return message.Author.IsBot == true;
        }

        private bool CheckIfTheMessageContainsCommand (SocketUserMessage message, out int commandPrefixPosition)
        {
            commandPrefixPosition = 0;

            return message.HasCharPrefix(BOT_COMMAND_PREFIX, ref commandPrefixPosition) == true;
        }

        private async Task GetCommandsModule ()
        {
            await Commands.AddModulesAsync(Assembly.GetEntryAssembly(), null);
        }

        private async Task StartBot ()
        {
            await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_WITH_BOT_TOKEN));
            await Client.StartAsync();
        }
    }
}