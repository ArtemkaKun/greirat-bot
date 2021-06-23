using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("shall")]
    public class ShowAllCommandsModule : BaseShowCommandsModule
    {
        [Command(COMMON_SHOW_COMMAND_NAME)]
        [Summary("Shows all today's orders")]
        public Task ShowTodayOrders ()
        {
            return ShowOrdersData(COMMON_SHOW_COMMAND_NAME, GetAllTodayOrders());
        }
        
        [Command(SORT_SHOW_COMMAND_NAME)]
        [Summary("Shows all today's (sorted)")]
        public Task ShowTodayOrdersSorted ()
        {
            return ShowOrdersData(SORT_SHOW_COMMAND_NAME, GetAllTodayOrders());
        }
        
        [Command(SUM_SHOW_COMMAND_NAME)]
        [Summary("Shows all today's (summary)")]
        public Task ShowTodayOrdersSummary ()
        {
            return ShowOrdersData(SUM_SHOW_COMMAND_NAME, GetAllTodayOrders());
        }

        private Queue<OrderData> GetAllTodayOrders ()
        {
            return Program.DBManager.GetTodayOrders();
        }
    }
}