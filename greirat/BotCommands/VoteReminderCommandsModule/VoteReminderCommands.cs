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
            string infoAboutReminder = Program.VoteRemindersController.GetVoteReminderInfo(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(string.IsNullOrEmpty(infoAboutReminder) == true ? VoteReminderModuleDatabase.NO_REMINDER_IN_CHANNEL_MESSAGE : infoAboutReminder);
        }

        [Command(CommandsDatabase.DELETE_COMMAND_NAME)]
        [Summary(VoteReminderModuleDatabase.DELETE_REMINDER_COMMAND_DESCRIPTION)]
        public Task DeleteChannelVoteReminder ()
        {
            bool isOperationSucceed = Program.VoteRemindersController.TryDeleteChannelVoteReminder(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(isOperationSucceed == true ? VoteReminderModuleDatabase.REMINDER_WAS_REMOVED_MESSAGE : VoteReminderModuleDatabase.NO_REMINDER_IN_CHANNEL_MESSAGE);
        }

        [Command(CommandsDatabase.UPDATE_COMMAND_NAME)]
        [Summary(VoteReminderModuleDatabase.UPDATE_REMINDER_COMMAND_DESCRIPTION)]
        public Task UpdateChannelVoteReminder (string remindTime, [Remainder] string remindMessage)
        {
            bool isOperationSucceed = Program.VoteRemindersController.TryUpdateChannelVoteReminder(Context, remindTime, remindMessage);

            return ReplyAsync(isOperationSucceed == true ? VoteReminderModuleDatabase.REMINDER_WAS_UPDATED_MESSAGE : VoteReminderModuleDatabase.NO_REMINDER_IN_CHANNEL_MESSAGE);
        }
    }
}