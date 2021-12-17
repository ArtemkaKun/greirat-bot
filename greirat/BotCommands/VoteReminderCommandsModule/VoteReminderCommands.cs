using System;
using System.Threading.Tasks;
using Discord.Commands;
using greirat;
using VoteReminderSystem;

namespace BotCommands
{
	[Group(VoteReminderModuleDatabase.VOTE_REMINDER_COMMANDS_GROUP_NAME)]
	public class VoteReminderCommands : ModuleBase<SocketCommandContext>
	{
		[Command(CommandsDatabase.SET_COMMAND_NAME)]
		[Summary(VoteReminderModuleDatabase.SET_REMINDER_COMMAND_DESCRIPTION)]
		public Task SetEverydayVoteReminder (string startTime, int durationInSeconds, string startMessage, string finishMessage)
		{
			if (TimeSpan.TryParse(startTime, out TimeSpan parsedTime) == true)
			{
				return ProceedVoteReminderCommandWithReply(Program.VoteRemindersController.TryStartNewVoteReminder, new VoteReminderInfo(parsedTime, startMessage, durationInSeconds, finishMessage));
			}

			return Task.CompletedTask;
		}

		[Command(CommandsDatabase.SHOW_COMMAND_NAME)]
		[Summary(VoteReminderModuleDatabase.SHOW_REMINDER_COMMAND_DESCRIPTION)]
		public Task ShowChannelVoteReminderData ()
		{
			return ProceedVoteReminderCommandWithReply(Program.VoteRemindersController.GetVoteReminderInfo);
		}

		[Command(CommandsDatabase.DELETE_COMMAND_NAME)]
		[Summary(VoteReminderModuleDatabase.DELETE_REMINDER_COMMAND_DESCRIPTION)]
		public Task DeleteChannelVoteReminder ()
		{
			return ProceedVoteReminderCommandWithReply(Program.VoteRemindersController.TryDeleteChannelVoteReminder);
		}

		//Logic was changed, need to update separate stuff with separate commands. 17.12.2021. Artem Yurchenko
		// [Command(CommandsDatabase.UPDATE_COMMAND_NAME)]
		// [Summary(VoteReminderModuleDatabase.UPDATE_REMINDER_COMMAND_DESCRIPTION)]
		// public Task UpdateChannelVoteReminder (string remindTime, [Remainder] string remindMessage)
		// {
		// 	return ProceedVoteReminderCommandWithReply(Program.VoteRemindersController.TryUpdateChannelVoteReminder, new VoteReminderInfo(remindTime, remindMessage));
		// }

		private Task ProceedVoteReminderCommandWithReply (Func<SocketCommandContext, string> actionToPerform)
		{
			string resultMessage = actionToPerform?.Invoke(Context);
			return ReplyAsync(resultMessage);
		}

		private Task ProceedVoteReminderCommandWithReply (Func<SocketCommandContext, VoteReminderInfo, string> actionToPerform, VoteReminderInfo reminderInfo)
		{
			string resultMessage = actionToPerform?.Invoke(Context, reminderInfo);
			return ReplyAsync(resultMessage);
		}
	}
}