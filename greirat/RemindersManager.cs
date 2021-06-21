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

        private static List<VoteReminder> ActiveReminders { get; set; } = new();

        public async Task StartRemindersFromDB ()
        {
            CollectRemindersFromDB();
            StartAllReminders();
            Console.WriteLine(REMINDERS_WERE_ACTIVATED_MESSAGE);

            await Task.Yield();
        }

        public bool TryStartNewReminder (SocketCommandContext context, string timeOfDayWhereRemind, string messageToRemind, int voteDurationInMinutes)
        {
            if (FindReminder(context.Guild.Id, context.Message.Channel.Id) != null)
            {
                return false;
            }

            if (voteDurationInMinutes == 0)
            {
                voteDurationInMinutes = 60;
            }

            VoteRemindData newReminderData = Program.DBManager.AddNewReminder(context, timeOfDayWhereRemind, messageToRemind, voteDurationInMinutes);
            VoteReminder newReminder = new(newReminderData);
            ActiveReminders.Add(newReminder);
            newReminder.TryStartReminderThread();

            return true;
        }

        public string GetReminderInfo (ulong guildID, ulong channelID)
        {
            VoteRemindData channelReminderInfo = FindReminder(guildID, channelID)?.ReminderData;

            return channelReminderInfo == null ? null : string.Format(REMINDER_INFO_MESSAGE, channelReminderInfo.TimeToRemind, channelReminderInfo.RemindMessage);
        }

        public bool TryDeleteChannelReminder (ulong guildID, ulong channelID)
        {
            VoteReminder channelReminderInfo = FindReminder(guildID, channelID);

            if (channelReminderInfo == null)
            {
                return false;
            }

            channelReminderInfo.CancelActualReminderThread();
            ActiveReminders.Remove(channelReminderInfo);
            Program.DBManager.DeleteReminder(channelReminderInfo.ReminderData);

            return true;
        }

        public bool TryUpdateChannelReminder (SocketCommandContext context, string timeOfDayWhereRemind, string messageToRemind)
        {
            VoteReminder reminderForThisChannel = FindReminder(context.Guild.Id, context.Message.Channel.Id);
            
            if (reminderForThisChannel == null)
            {
                return false;
            }

            reminderForThisChannel.ReminderData.TimeToRemind = timeOfDayWhereRemind;
            reminderForThisChannel.ReminderData.RemindMessage = messageToRemind;
            Program.DBManager.UpdateReminder(reminderForThisChannel.ReminderData);
            reminderForThisChannel.TryStartReminderThread();

            return true;
        }

        private void CollectRemindersFromDB ()
        {
            Stack<VoteRemindData> remindersCollection = Program.DBManager.GetAllRemindersFromDB();
            ActiveReminders = new List<VoteReminder>(remindersCollection.Count);

            while (remindersCollection.Count > 0)
            {
                ActiveReminders.Add(new VoteReminder(remindersCollection.Pop()));
            }
        }

        private void StartAllReminders ()
        {
            for (int reminderPointer = 0; reminderPointer < ActiveReminders.Count; reminderPointer++)
            {
                ActiveReminders[reminderPointer].TryStartReminderThread();
            }
        }

        private VoteReminder FindReminder (ulong guildID, ulong channelID)
        {
            for (int reminderPointer = 0; reminderPointer < ActiveReminders.Count; reminderPointer++)
            {
                VoteReminder currentReminder = ActiveReminders[reminderPointer];

                if ((currentReminder.ReminderData.GuildID == guildID) && (currentReminder.ReminderData.ChannelID == channelID))
                {
                    return currentReminder;
                }
            }

            return null;
        }
    }
}