using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group(VoteReminderModuleDatabase.VOTE_REMINDER_COMMANDS_GROUP_NAME)]
    public class VoteReminderCommands : ModuleBase<SocketCommandContext>
    {
        [Command(CommandsDatabase.SET_COMMAND_NAME)]
        [Summary(VoteReminderModuleDatabase.SET_REMINDER_COMMAND_DESCRIPTION)]
        public Task SetEverydayVoteReminder (string remindTime, [Optional] int voteDurationInMinutes, [Remainder] string remindMessage)
        {
            string resultMessage = Program.VoteRemindersController.TryStartNewVoteReminder(Context, remindTime, remindMessage, voteDurationInMinutes);
            return ReplyAsync(resultMessage);
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
            string resultMessage = Program.VoteRemindersController.TryUpdateChannelVoteReminder(Context, remindTime, remindMessage);
            return ReplyAsync(resultMessage);
        }

        private Task ProceedVoteReminderCommandWithReply (Func<SocketCommandContext, string> actionToPerform)
        {
            string resultMessage = actionToPerform?.Invoke(Context);
            return ReplyAsync(resultMessage);
        }
    }
}