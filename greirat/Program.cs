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
        public static List<OrdersReminder> ActiveReminders { get; private set; }
        
        private static void Main ()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync ()
        {
            await Bot.Initialize();
            DBManager.EnsureThatDBIsCreated();
            await StartReminders();
            await Task.Delay(-1);
        }

        private static async Task StartReminders ()
        {
            CollectRemindersFromDB();
            StartAllReminders();
            Console.WriteLine(REMINDERS_WERE_ACTIVATED_MESSAGE);

            await Task.Yield();
        }

        private static void CollectRemindersFromDB ()
        {
            Stack<FoodRemindData> remindersCollection = DBManager.GetAllRemindersFromDB();
            ActiveReminders = new List<OrdersReminder>(remindersCollection.Count);
            
            while (remindersCollection.Count > 0)
            {
                ActiveReminders.Add(new OrdersReminder(remindersCollection.Pop()));
            }
        }

        private static void StartAllReminders ()
        {
            for (int reminderPointer = 0; reminderPointer < ActiveReminders.Count; reminderPointer++)
            {
                ActiveReminders[reminderPointer].TryStartReminderThread();
            }
        }
    }
}   