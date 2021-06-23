using System.Threading.Tasks;

namespace greirat
{
    internal static class Program
    {
        public static DiscordBot BotClient { get; } = new();
        public static DB DBManager { get; } = new();
        public static VoteRemindersManager VoteRemindersController { get; } = new();

        private static void Main ()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync ()
        {
            await BotClient.Initialize();
            DBManager.EnsureThatDBIsCreated();
            await VoteRemindersController.StartRemindersFromDB();
            await Task.Delay(-1);
        }
    }
}