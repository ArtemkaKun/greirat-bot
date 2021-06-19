using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    public class RemindersManager
    {
        private const string REMINDERS_WERE_ACTIVATED_MESSAGE = "All reminders from DB were activated";
        private const string REMINDER_INFO_MESSAGE = "```Every day (except weekends) at {0} send message '{1}' to the chat```";

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
            if (FindReminder(context.Guild.Id, context.Message.Channel.Id) == null)
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
            FoodRemindData channelReminderInfo = FindReminder(guildID, channelID).ReminderData;

            return channelReminderInfo == null ? null : string.Format(REMINDER_INFO_MESSAGE, channelReminderInfo.TimeToRemind, channelReminderInfo.RemindMessage);
        }

        public bool TryDeleteChannelReminder (ulong guildID, ulong channelID)
        {
            OrdersReminder channelReminderInfo = FindReminder(guildID, channelID);

            if (channelReminderInfo == null)
            {
                return false;
            }

            ActiveReminders.Remove(channelReminderInfo);

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

        private OrdersReminder FindReminder (ulong guildID, ulong channelID)
        {
            for (int reminderPointer = 0; reminderPointer < ActiveReminders.Count; reminderPointer++)
            {
                OrdersReminder currentReminder = ActiveReminders[reminderPointer];

                if ((currentReminder.ReminderData.GuildID == guildID) && (currentReminder.ReminderData.ChannelID == channelID))
                {
                    return currentReminder;
                }
            }

            return null;
        }
    }
}