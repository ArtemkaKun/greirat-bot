using System.Threading.Tasks;

namespace greirat
{
    internal class Program
    {
        public static DiscordBot Bot { get; private set; } = new();
        
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