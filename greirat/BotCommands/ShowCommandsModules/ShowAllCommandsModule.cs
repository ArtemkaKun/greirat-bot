using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using greirat;

namespace BotCommands
{
    [Group(ShowCommandsModuleDatabase.SHOW_ALL_COMMANDS_GROUP_NAME)]
    public class ShowAllCommandsModule : BaseShowCommandsModule
    {
        [Command(COMMON_SHOW_COMMAND_NAME)]
        [Summary(ShowCommandsModuleDatabase.SHOW_TODAY_ORDERS_COMMAND_DESCRIPTION)]
        public Task ShowTodayOrders ()
        {
            return ShowOrdersData(COMMON_SHOW_COMMAND_NAME, GetAllTodayOrders());
        }
        
        [Command(SORT_SHOW_COMMAND_NAME)]
        [Summary(ShowCommandsModuleDatabase.SHOW_TODAY_ORDERS_SORTED_COMMAND_DESCRIPTION)]
        public Task ShowTodayOrdersSorted ()
        {
            return ShowOrdersData(SORT_SHOW_COMMAND_NAME, GetAllTodayOrders());
        }
        
        [Command(SUM_SHOW_COMMAND_NAME)]
        [Summary(ShowCommandsModuleDatabase.SHOW_TODAY_ORDERS_SUMMARY_COMMAND_DESCRIPTION)]
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