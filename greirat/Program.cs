using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace greirat
{
    internal class Program
    {
        private const string REMINDERS_WERE_ACTIVATED_MESSAGE = "All reminders from DB were activated";
        
        public static DiscordBot Bot { get; private set; } = new();
        public static DB DBManager { get; private set; } = new();
        public static List<OrdersReminder> ActiveReminders { get; private set; } = new();
        
        private static void Main ()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync ()
        {
            await Bot.Initialize();
            await StartReminders();
            await Task.Delay(-1);
        }

        private static async Task StartReminders ()
        {
            Stack<FoodRemindData> remindersCollection = DBManager.GetAllRemindersFromDB();

            while (remindersCollection.Count > 0)
            {
                ActiveReminders.Add(new OrdersReminder(remindersCollection.Pop()));
            }
            
            Console.WriteLine(REMINDERS_WERE_ACTIVATED_MESSAGE);

            await Task.Yield();
        }
    }
}   