using System.Threading.Tasks;

namespace greirat
{
    internal class Program
    {
        private DiscordBot Bot { get; set; } = new();
        
        private static void Main ()
        {
            new Program().MainAsync().GetAwaiter().GetResult();
        }

        private async Task MainAsync ()
        {
            await Bot.Initialize();
            await Task.Delay(-1);
        }
    }
}