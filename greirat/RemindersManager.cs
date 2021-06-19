using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace greirat
{
    public class RemindersManager
    {
        private const string REMINDERS_WERE_ACTIVATED_MESSAGE = "All reminders from DB were activated";
        
        public static List<OrdersReminder> ActiveReminders { get; private set; }
        
        public async Task StartRemindersFromDB ()
        {
            CollectRemindersFromDB();
            StartAllReminders();
            Console.WriteLine(REMINDERS_WERE_ACTIVATED_MESSAGE);

            await Task.Yield();
        }

        private static void CollectRemindersFromDB ()
        {
            Stack<FoodRemindData> remindersCollection = Program.DBManager.GetAllRemindersFromDB();
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