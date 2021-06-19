using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("remind")]
    public class ReminderCommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string REMINDER_WAS_SET_MESSAGE = "Reminder was set on {0} everyday (except weekends)";
        private const string CANNOT_SET_REMINDER_MESSAGE = "Cannot set reminder for this channel";
        
        [Command("-set")]
        [Summary("Sets reminder about of food orders")]
        public Task SetEverydayReminder (string timeOfDayWhereRemind, [Remainder] string messageToRemind)
        {
            bool isOperationSucceed = Program.RemindersOrchestrator.TryStartNewReminder(Context, timeOfDayWhereRemind, messageToRemind);

            return ReplyAsync(isOperationSucceed == true ? string.Format(REMINDER_WAS_SET_MESSAGE, timeOfDayWhereRemind) : CANNOT_SET_REMINDER_MESSAGE);
        }
    }
}