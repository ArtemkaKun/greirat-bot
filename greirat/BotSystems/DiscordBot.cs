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

        private DiscordSocketClient Client { get; set; } = new();
        private CommandService Commands { get; set; } = new();

        public async Task Initialize ()
        {
            SubscribeOnClientEvents();
            await GetCommandsModule();
        }

        public async Task StartBot ()
        {
            await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_WITH_BOT_TOKEN));
            await Client.StartAsync();
        }

        public async Task SendMessage (ulong guildID, ulong channelID, string messageText)
        {
            await Client.GetGuild(guildID).GetTextChannel(channelID).SendMessageAsync(messageText);
        }

        private void SubscribeOnClientEvents ()
        {
            Client.Log += ProceedLogMessage;
            Client.MessageReceived += ProceedReceivedMessage;
        }

        private Task ProceedLogMessage (LogMessage msg)
        {
            Console.WriteLine(msg.ToString());

            return Task.CompletedTask;
        }

        private async Task ProceedReceivedMessage (SocketMessage message)
        {
            if (message is not SocketUserMessage userMessage)
            {
                return;
            }

            if (CheckIfMessageFromBot(userMessage) == true)
            {
                return;
            }

            if (CheckIfTheMessageContainsCommand(userMessage, out int commandPrefixPosition) == false)
            {
                return;
            }

            SocketCommandContext context = new(Client, userMessage);
            await Commands.ExecuteAsync(context, commandPrefixPosition, null);
        }

        private bool CheckIfMessageFromBot (SocketUserMessage message)
        {
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
    }
}