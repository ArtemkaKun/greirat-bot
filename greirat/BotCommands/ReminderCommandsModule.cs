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
            FoodRemindData newReminderID = Program.DBManager.AddNewReminder(Context, timeOfDayWhereRemind, messageToRemind);
            new OrdersReminder(newReminderID).TryStartReminderThread();

            return ReplyAsync(string.Format(CommandsDatabase.REMINDER_WAS_SET_MESSAGE, timeOfDayWhereRemind));
        }
    }
}