using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    public class RemindersManager
    {
        private const string REMINDERS_WERE_ACTIVATED_MESSAGE = "All reminders from DB were activated";

        private static List<OrdersReminder> ActiveReminders { get; set; }
        
        public async Task StartRemindersFromDB ()
        {
            CollectRemindersFromDB();
            StartAllReminders();
            Console.WriteLine(REMINDERS_WERE_ACTIVATED_MESSAGE);

            await Task.Yield();
        }

        public bool TryStartNewReminder (SocketCommandContext context, string timeOfDayWhereRemind, string messageToRemind)
        {
            if (string.IsNullOrEmpty(GetReminderInfo(context.Guild.Id, context.Message.Channel.Id)) == false)
            {
                return false;
            }
            
            FoodRemindData newReminderID = Program.DBManager.AddNewReminder(context, timeOfDayWhereRemind, messageToRemind);
            OrdersReminder newReminder = new(newReminderID);
            ActiveReminders.Add(newReminder);
            newReminder.TryStartReminderThread();

            return true;
        }

        public string GetReminderInfo (ulong guildID, ulong channelID)
        {
            for (int reminderPointer = 0; reminderPointer < ActiveReminders.Count; reminderPointer++)
            {
                FoodRemindData currentReminderData = ActiveReminders[reminderPointer].ReminderData;
                
                if ((currentReminderData.GuildID == guildID) && (currentReminderData.ChannelID == channelID))
                {
                    return $"```Every day (except weekends) at {currentReminderData.TimeToRemind} send message '{currentReminderData.RemindMessage}' to the chat```";
                }
            }

            return null;
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