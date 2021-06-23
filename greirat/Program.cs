using System.Threading.Tasks;

namespace greirat
{
    internal static class Program
    {
        public static DiscordBot BotClient { get; } = new();
        public static DB DBManager { get; } = new();
        public static VoteRemindersManager VoteVoteRemindersOrchestrator { get; private set; } = new();

        private static void Main ()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync ()
        {
            await BotClient.Initialize();
            DBManager.EnsureThatDBIsCreated();
            await VoteVoteRemindersOrchestrator.StartRemindersFromDB();
            await Task.Delay(-1);
        }
    }
}