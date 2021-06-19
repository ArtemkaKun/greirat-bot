using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("remind")]
    public class ReminderCommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string REMINDER_WAS_SET_MESSAGE = "Reminder was set on {0} everyday (except weekends)";
        private const string CANNOT_SET_REMINDER_MESSAGE = "Cannot set reminder for this channel";
        private const string NO_REMINDER_IN_CHANNEL_MESSAGE = "No reminders in channet yet";
        private const string REMINDER_WAS_REMOVED_MESSAGE = "Reminder was successfully removed";
        
        [Command("-set")]
        [Summary("Sets reminder about of food vote")]
        public Task SetEverydayReminder (string timeOfDayWhereRemind, [Remainder] string messageToRemind)
        {
            bool isOperationSucceed = Program.RemindersOrchestrator.TryStartNewReminder(Context, timeOfDayWhereRemind, messageToRemind);

            return ReplyAsync(isOperationSucceed == true ? string.Format(REMINDER_WAS_SET_MESSAGE, timeOfDayWhereRemind) : CANNOT_SET_REMINDER_MESSAGE);
        }
        
        [Command("-show")]
        [Summary("Show reminder data for this channel")]
        public Task ShowChannelReminderData ()
        {
            string infoAboutReminder = Program.RemindersOrchestrator.GetReminderInfo(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(string.IsNullOrEmpty(infoAboutReminder) == true ? NO_REMINDER_IN_CHANNEL_MESSAGE : infoAboutReminder);
        }
        
        [Command("-del")]
        [Summary("Delete reminder for this channel")]
        public Task DeleteChannelReminder ()
        {
            bool isOperationSucceed = Program.RemindersOrchestrator.TryDeleteChannelReminder(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(isOperationSucceed == true ? REMINDER_WAS_REMOVED_MESSAGE : NO_REMINDER_IN_CHANNEL_MESSAGE);
        }
    }
}