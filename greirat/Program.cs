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
            ProceedSyncStart();
            MainAsync().GetAwaiter().GetResult();
        }

        private static void ProceedSyncStart ()
        {
            DBManager.EnsureDBIsCreated();
            VoteRemindersController.StartRemindersFromDB();
        }
        
        private static async Task MainAsync ()
        {
            await BotClient.Initialize();
            await Task.Delay(-1);
        }
    }
}