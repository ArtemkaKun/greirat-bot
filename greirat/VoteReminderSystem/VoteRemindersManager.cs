using System;
using System.Collections.Generic;
using DBSystem;
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
		private const string VOTE_REMINDER_ALREADY_EXISTS_MESSAGE = "Vote reminder already exists for this channel. Try delete it first or update its info.";

		private static List<VoteReminder> ActiveReminders { get; set; } = new();

		public void StartRemindersFromDB ()
		{
			CollectRemindersFromDB();
			StartAllReminders();
			Console.WriteLine(REMINDERS_WERE_ACTIVATED_MESSAGE, ActiveReminders.Count.ToString());
		}

		private void CollectRemindersFromDB ()
		{
			Stack<VoteData> remindersCollection = Program.DBManager.GetAllRemindersFromDB();
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

		public string TryStartNewVoteReminder (SocketCommandContext commandContext, VoteReminderInfo reminderInfo)
		{
			if (FindReminder(commandContext) != null)
			{
				return VOTE_REMINDER_ALREADY_EXISTS_MESSAGE;
			}

			VoteReminder newReminder = CreateNewReminder(commandContext, reminderInfo);
			newReminder.TryStartReminderThread();

			return string.Format(REMINDER_INFO_MESSAGE, reminderInfo.StartTime, reminderInfo.StartMessage);
		}

		private VoteReminder CreateNewReminder (SocketCommandContext commandContext, VoteReminderInfo reminderInfo)
		{
			VoteData newReminderData = Program.DBManager.AddNewReminder(commandContext, reminderInfo);
			VoteReminder newReminder = new(newReminderData);
			ActiveReminders.Add(newReminder);

			return newReminder;
		}

		public string GetVoteReminderInfo (SocketCommandContext commandContext)
		{
			VoteData channelReminderInfo = FindReminder(commandContext)?.ReminderData;
			return channelReminderInfo == null ? NO_REMINDER_IN_CHANNEL_MESSAGE : string.Format(REMINDER_INFO_MESSAGE, channelReminderInfo.StartTime, channelReminderInfo.StartMessage);
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

		//Logic was changed, need to update separate stuff with separate commands. 17.12.2021. Artem Yurchenko
		// public string TryUpdateChannelVoteReminder (SocketCommandContext commandContext, VoteReminderInfo reminderInfo)
		// {
		// 	VoteReminder reminderForThisChannel = FindReminder(commandContext);
		//
		// 	if (reminderForThisChannel == null)
		// 	{
		// 		return NO_REMINDER_IN_CHANNEL_MESSAGE;
		// 	}
		//
		// 	reminderForThisChannel.UpdateReminderData(reminderInfo);
		// 	Program.DBManager.UpdateReminder(reminderForThisChannel.ReminderData);
		// 	reminderForThisChannel.TryStartReminderThread();
		//
		// 	return REMINDER_WAS_UPDATED_MESSAGE;
		// }

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