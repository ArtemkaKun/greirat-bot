using System;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace greirat
{
    internal class Program
    {
        private const string ENVIRONMENT_VARIABLE_WITH_BOT_TOKEN = "FOOD_BOT_TOKEN";
        
        private DiscordSocketClient Client { get; set; } = new ();
        
        private static void Main ()
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync()
        {
            SubscribeOnClientEvents();
            await StartBot();
            await Task.Delay(-1);
        }

        private void SubscribeOnClientEvents ()
        {
            Client.Log += Log;
        }

        private async Task StartBot ()
        {
            await Client.LoginAsync(TokenType.Bot, Environment.GetEnvironmentVariable(ENVIRONMENT_VARIABLE_WITH_BOT_TOKEN));
            await Client.StartAsync();
        }
        
        private Task Log(LogMessage msg)
        {
            Console.WriteLine(msg.ToString());
            
            return Task.CompletedTask;
        }
    }
}