using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

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

        public bool TryStartNewReminder (SocketCommandContext context, string timeOfDayWhereRemind, string messageToRemind)
        {
            if (Program.DBManager.CheckIfSimilarReminderAlreadyExists(context) == true)
            {
                return false;
            }
            
            FoodRemindData newReminderID = Program.DBManager.AddNewReminder(context, timeOfDayWhereRemind, messageToRemind);
            OrdersReminder newReminder = new(newReminderID);
            ActiveReminders.Add(newReminder);
            newReminder.TryStartReminderThread();

            return true;
        }

        private void CollectRemindersFromDB ()
        {
            Stack<FoodRemindData> remindersCollection = Program.DBManager.GetAllRemindersFromDB();
            ActiveReminders = new List<OrdersReminder>(remindersCollection.Count);
            
            while (remindersCollection.Count > 0)
            {
                ActiveReminders.Add(new OrdersReminder(remindersCollection.Pop()));
            }
        }

        private void StartAllReminders ()
        {
            for (int reminderPointer = 0; reminderPointer < ActiveReminders.Count; reminderPointer++)
            {
                ActiveReminders[reminderPointer].TryStartReminderThread();
            }
        }
    }
}