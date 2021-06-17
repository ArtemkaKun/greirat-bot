using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using greirat.Helpers;

namespace greirat
{
    [Group("shall")]
    public class ShowCommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string COMMON_SHOW_COMMAND_NAME = "";
        private const string SORT_SHOW_COMMAND_NAME = "-sort";
        private const string SUM_SHOW_COMMAND_NAME = "-sum";
        
        private OrderDataAsciiTableConverter OrdersOutputMaintainer { get; set; } = new();
        private Dictionary<string, Func<Queue<OrderData>, StringBuilder>> ShowAllOptionsFunctions { get; set; }

        public ShowCommandsModule ()
        {
            ShowAllOptionsFunctions = new Dictionary<string, Func<Queue<OrderData>, StringBuilder>>
            {
                {COMMON_SHOW_COMMAND_NAME, OrdersOutputMaintainer.FormOrdersShowData},
                {SORT_SHOW_COMMAND_NAME, OrdersOutputMaintainer.FormOrdersSortedShowData},
                {SUM_SHOW_COMMAND_NAME, OrdersOutputMaintainer.FormOrdersSummaryShowData}
            };
        }

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

        private Task ShowOrdersData (string command)
        {
            Queue<OrderData> todayOrders = Program.DBManager.GetTodayOrders();

            return ReplyAsync(todayOrders.Count == 0 ? CommandsDatabase.NOTHING_TO_SHOW_MESSAGE : ShowAllOptionsFunctions[command]?.Invoke(todayOrders).ToString());
        }
    }
}