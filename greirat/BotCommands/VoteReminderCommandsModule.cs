using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("voteReminder")]
    public class VoteReminderCommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string REMINDER_WAS_SET_MESSAGE = "Reminder was set on {0} everyday (except weekends)";
        private const string CANNOT_SET_REMINDER_MESSAGE = "Cannot set reminder for this channel";
        private const string NO_REMINDER_IN_CHANNEL_MESSAGE = "No reminders in channet yet";
        private const string REMINDER_WAS_REMOVED_MESSAGE = "Reminder was successfully removed";
        private const string REMINDER_WAS_UPDATED_MESSAGE = "Reminder was successfully updated";
        
        [Command("-set")]
        [Summary("Sets reminder about of food vote")]
        public Task SetEverydayVoteReminder (string timeOfDayWhereRemind, [Optional] int durationOfVoteInMinutes, [Remainder] string messageToRemind)
        {
            bool isOperationSucceed = Program.VoteVoteRemindersOrchestrator.TryStartNewVoteReminder(Context, timeOfDayWhereRemind, messageToRemind, durationOfVoteInMinutes);

            return ReplyAsync(isOperationSucceed == true ? string.Format(REMINDER_WAS_SET_MESSAGE, timeOfDayWhereRemind) : CANNOT_SET_REMINDER_MESSAGE);
        }
        
        [Command("-show")]
        [Summary("Show reminder data for this channel")]
        public Task ShowChannelVoteReminderData ()
        {
            string infoAboutReminder = Program.VoteVoteRemindersOrchestrator.GetVoteReminderInfo(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(string.IsNullOrEmpty(infoAboutReminder) == true ? NO_REMINDER_IN_CHANNEL_MESSAGE : infoAboutReminder);
        }
        
        [Command("-del")]
        [Summary("Delete reminder for this channel")]
        public Task DeleteChannelVoteReminder ()
        {
            bool isOperationSucceed = Program.VoteVoteRemindersOrchestrator.TryDeleteChannelVoteReminder(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(isOperationSucceed == true ? REMINDER_WAS_REMOVED_MESSAGE : NO_REMINDER_IN_CHANNEL_MESSAGE);
        }
        
        [Command("-upd")]
        [Summary("Update reminder for this channel with a new data")]
        public Task UpdateChannelVoteReminder (string timeOfDayWhereRemind, [Remainder] string messageToRemind)
        {
            bool isOperationSucceed = Program.VoteVoteRemindersOrchestrator.TryUpdateChannelVoteReminder(Context, timeOfDayWhereRemind, messageToRemind);

            return ReplyAsync(isOperationSucceed == true ? REMINDER_WAS_UPDATED_MESSAGE : NO_REMINDER_IN_CHANNEL_MESSAGE);
        }
    }
}