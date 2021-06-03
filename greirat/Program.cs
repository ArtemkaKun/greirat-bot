using System;
using System.Reflection;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;

namespace greirat
{
    internal class Program
    {
        private const string ENVIRONMENT_VARIABLE_WITH_BOT_TOKEN = "FOOD_BOT_TOKEN";
        private const char BOT_COMMAND_PREFIX = '!';

        private DiscordSocketClient Client { get; set; } = new();
        private CommandService Commands { get; set; } = new ();

        private static void Main ()
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync ()
        {
            SubscribeOnClientEvents();
            await GetCommandsModule();
            await StartBot();
            await Task.Delay(-1);
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

            if (CheckIfTheMessageContainsCommand(message, out int commandPrefixPosition))
            {
                return;
            }

            SocketCommandContext context = new(Client, message);
            await Commands.ExecuteAsync(context, commandPrefixPosition, null);
        }

        private bool CheckIfTheMessageContainsCommand (SocketUserMessage message, out int commandPrefixPosition)
        {
            commandPrefixPosition = 0;

            return (message.Author.IsBot == false) && (message.HasCharPrefix(BOT_COMMAND_PREFIX, ref commandPrefixPosition) == true);
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