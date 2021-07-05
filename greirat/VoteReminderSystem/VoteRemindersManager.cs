﻿using System;
using System.Collections.Generic;
using Discord.Commands;
using greirat;

namespace VoteReminderSystem
{
	public class VoteRemindersManager
	{
		private const string REMINDERS_WERE_ACTIVATED_MESSAGE = "All reminders from DB were activated ({0} reminders)";
		private const string REMINDER_INFO_MESSAGE = "```Every day (except weekends) at {0} send message '{1}' to the chat```";
		private const string NO_REMINDER_IN_CHANNEL_MESSAGE = "No reminders in channel yet";
		private const string REMINDER_WAS_REMOVED_MESSAGE = "Reminder was successfully removed";
		private const string REMINDER_WAS_UPDATED_MESSAGE = "Reminder was successfully updated";

		private static List<VoteReminder> ActiveReminders { get; set; } = new();

		public void StartRemindersFromDB ()
		{
			CollectRemindersFromDB();
			StartAllReminders();
			Console.WriteLine(REMINDERS_WERE_ACTIVATED_MESSAGE, ActiveReminders.Count.ToString());
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
 
		public string TryStartNewVoteReminder (SocketCommandContext commandContext, SimpleReminderInfo reminderInfo)
		{
			if (FindReminder(commandContext) != null)
			{
				return "Vote reminder already exists for this channel. Try delete it first or update its info.";
			}

			VoteRemindData newReminderData = Program.DBManager.AddNewReminder(commandContext, reminderInfo.RemindTime, reminderInfo.RemindMessage, 60);
			VoteReminder newReminder = new(newReminderData);
			ActiveReminders.Add(newReminder);
			newReminder.TryStartReminderThread();

			return string.Format(REMINDER_INFO_MESSAGE, reminderInfo.RemindTime, reminderInfo.RemindMessage);
		}

		public string GetVoteReminderInfo (SocketCommandContext commandContext)
		{
			VoteRemindData channelReminderInfo = FindReminder(commandContext)?.ReminderData;
			return channelReminderInfo == null ? NO_REMINDER_IN_CHANNEL_MESSAGE : string.Format(REMINDER_INFO_MESSAGE, channelReminderInfo.TimeToRemind, channelReminderInfo.RemindMessage);
		}

		public string TryDeleteChannelVoteReminder (SocketCommandContext commandContext)
		{
			VoteReminder channelReminderInfo = FindReminder(commandContext);

			if (channelReminderInfo == null)
			{
				return NO_REMINDER_IN_CHANNEL_MESSAGE;
			}

			channelReminderInfo.CancelActualReminderThread();
			ActiveReminders.Remove(channelReminderInfo);
			Program.DBManager.DeleteReminder(channelReminderInfo.ReminderData);

			return REMINDER_WAS_REMOVED_MESSAGE;
		}

		public string TryUpdateChannelVoteReminder (SocketCommandContext commandContext, SimpleReminderInfo reminderInfo)
		{
			VoteReminder reminderForThisChannel = FindReminder(commandContext);

			if (reminderForThisChannel == null)
			{
				return NO_REMINDER_IN_CHANNEL_MESSAGE;
			}

			reminderForThisChannel.ReminderData.TimeToRemind = reminderInfo.RemindTime;
			reminderForThisChannel.ReminderData.RemindMessage = reminderInfo.RemindMessage;
			Program.DBManager.UpdateReminder(reminderForThisChannel.ReminderData);
			reminderForThisChannel.TryStartReminderThread();

			return REMINDER_WAS_UPDATED_MESSAGE;
		}

		private VoteReminder FindReminder (SocketCommandContext context)
		{
			for (int reminderPointer = 0; reminderPointer < ActiveReminders.Count; reminderPointer++)
			{
				VoteReminder currentReminder = ActiveReminders[reminderPointer];

				if ((currentReminder.ReminderData.GuildID == context.Guild.Id) && (currentReminder.ReminderData.ChannelID == context.Message.Channel.Id))
				{
					return currentReminder;
				}
			}

			return null;
		}
	}
}