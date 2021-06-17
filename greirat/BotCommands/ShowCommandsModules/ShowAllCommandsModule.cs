using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("shall")]
    public class ShowAllCommandsModule : BaseShowCommandsModule
    {
        [Command(COMMON_SHOW_COMMAND_NAME)]
        [Summary("Shows all orders that was made today")]
        public Task ShowTodayOrders ()
        {
            return ShowOrdersData(COMMON_SHOW_COMMAND_NAME);
        }
        
        [Command(SORT_SHOW_COMMAND_NAME)]
        [Summary("Shows all orders that was made today (sorted a -> z)")]
        public Task ShowTodayOrdersSorted ()
        {
            return ShowOrdersData(SORT_SHOW_COMMAND_NAME);
        }
        
        [Command(SUM_SHOW_COMMAND_NAME)]
        [Summary("Shows all orders that was made today (summary mode)")]
        public Task ShowTodayOrdersSummary ()
        {
            return ShowOrdersData(SUM_SHOW_COMMAND_NAME);
        }
    }
}