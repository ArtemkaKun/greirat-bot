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
            ProceedSyncPreparations();
            ProceedAsyncPreparations().GetAwaiter().GetResult();
            StartBot().GetAwaiter().GetResult();
        }

        private static void ProceedSyncPreparations ()
        {
            DBManager.EnsureDBIsCreated();
            VoteRemindersController.StartRemindersFromDB();
        }
        
        private static async Task ProceedAsyncPreparations ()
        {
            await BotClient.Initialize();
        }

        private static async Task StartBot ()
        {
            await BotClient.StartBot();
            await Task.Delay(-1);
        }
    }
}