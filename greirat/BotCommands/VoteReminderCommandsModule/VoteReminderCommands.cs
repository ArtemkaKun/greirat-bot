using System;
using System.Threading.Tasks;
using Discord.Commands;
using greirat;

namespace BotCommands
{
    [Group(VoteReminderModuleDatabase.VOTE_REMINDER_COMMANDS_GROUP_NAME)]
    public class VoteReminderCommands : ModuleBase<SocketCommandContext>
    {
        [Command(CommandsDatabase.SET_COMMAND_NAME)]
        [Summary(VoteReminderModuleDatabase.SET_REMINDER_COMMAND_DESCRIPTION)]
        public Task SetEverydayVoteReminder (string remindTime, [Remainder] string remindMessage)
        {
            return ProceedVoteReminderCommandWithReply(Program.VoteRemindersController.TryStartNewVoteReminder, new SimpleReminderInfo(remindTime, remindMessage));
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

        [Command(CommandsDatabase.UPDATE_COMMAND_NAME)]
        [Summary(VoteReminderModuleDatabase.UPDATE_REMINDER_COMMAND_DESCRIPTION)]
        public Task UpdateChannelVoteReminder (string remindTime, [Remainder] string remindMessage)
        {
            return ProceedVoteReminderCommandWithReply(Program.VoteRemindersController.TryUpdateChannelVoteReminder, new SimpleReminderInfo(remindTime, remindMessage));
        }

        private Task ProceedVoteReminderCommandWithReply (Func<SocketCommandContext, string> actionToPerform)
        {
            string resultMessage = actionToPerform?.Invoke(Context);
            return ReplyAsync(resultMessage);
        }

        private Task ProceedVoteReminderCommandWithReply (Func<SocketCommandContext, SimpleReminderInfo, string> actionToPerform, SimpleReminderInfo reminderInfo)
        {
            string resultMessage = actionToPerform?.Invoke(Context, reminderInfo);
            return ReplyAsync(resultMessage);
        }
    }
}