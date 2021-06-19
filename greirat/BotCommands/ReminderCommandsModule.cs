using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("remind")]
    public class ReminderCommandsModule : ModuleBase<SocketCommandContext>
    {
        [Command("-set")]
        [Summary("Sets reminder about of food orders")]
        public Task SetEverydayReminder (string timeOfDayWhereRemind, [Remainder] string messageToRemind)
        {
            bool isOperationSucceed = Program.RemindersOrchestrator.TryStartNewReminder(Context, timeOfDayWhereRemind, messageToRemind);

            return ReplyAsync(isOperationSucceed == true ? string.Format(CommandsDatabase.REMINDER_WAS_SET_MESSAGE, timeOfDayWhereRemind) : "Cannot set reminder for this channel");
        }
    }
}