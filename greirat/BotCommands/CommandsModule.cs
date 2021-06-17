using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using greirat.Helpers;

namespace greirat
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE = "Order was successfully ";
        private const string ORDER_WAS_SAVED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "proceeded";
        private const string ORDER_WAS_UPDATED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "updated";
        private const string ORDER_WAS_REMOVED = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "removed";
        private const string ORDER_UPDATE_FAILED = "Failed to update order";
        private const string ORDER_DELETE_FAILED = "Failed to remove order";
        private const string NOTHING_TO_SHOW_MESSAGE = "Nothing to show";
        private const string REMINDER_WAS_SET_MESSAGE = "Reminder was set on {0} everyday (except weekends)";

        private OrderDataAsciiTableConverter OrdersOutputMaintainer { get; set; } = new();
        private HelpInfoAsciiTableConverter HelpOutputMaintainer { get; set; } = new();
        
        [Command("help")]
        [Alias("h")]
        [Summary("Shows bot's commands")]
        public Task ShowHelpMessage ()
        {
            return ReplyAsync(HelpOutputMaintainer.HelpInfoInTableForm);
        }
        
        [Command("makeOrder")]
        [Alias("mko")]
        [Summary("Creates a new order")]
        public Task CreateNewOrder ([Remainder] string orderText)
        {
            Program.DBManager.AddNewOrder(Context.Message.Author.Username, orderText);

            return ReplyAsync(ORDER_WAS_SAVED_MESSAGE);
        }

        [Command("showMyTodayOrders")]
        [Alias("shmy")]
        [Summary("Shows your today's orders")]
        public Task ShowUserTodayOrders ()
        {
            Queue<OrderData> todayOrders = Program.DBManager.GetTodayOrders(Context.Message.Author.Username);

            return ReplyAsync(todayOrders.Count == 0 ? NOTHING_TO_SHOW_MESSAGE : OrdersOutputMaintainer.FormOrdersShowData(todayOrders).ToString());
        }

        [Command("updateOrder")]
        [Alias("updo")]
        [Summary("Updates order with provided text.")]
        public Task UpdateUserOrder (int idOfOrder, [Remainder] string newOrderText)
        {
            bool updateOperationResult = Program.DBManager.TryUpdateOrderData(Context.Message.Author.Username, idOfOrder, newOrderText);

            return ReplyAsync(updateOperationResult == true ? ORDER_WAS_UPDATED_MESSAGE : ORDER_UPDATE_FAILED);
        }

        [Command("deleteOrder")]
        [Alias("delo")]
        [Summary("Deletes order.")]
        public Task DeleteOrder (int idOfOrder)
        {
            bool deleteOperationResult = Program.DBManager.TryDeleteOrderData(Context.Message.Author.Username, idOfOrder);

            return ReplyAsync(deleteOperationResult == true ? ORDER_WAS_REMOVED : ORDER_DELETE_FAILED);
        }

        [Command("setEverydayReminder")]
        [Alias("remind")]
        [Summary("Sets reminder about of food orders")]
        public Task SetEverydayReminder (string timeOfDayWhereRemind, [Remainder] string messageToRemind)
        {
            FoodRemindData newReminderID = Program.DBManager.AddNewReminder(Context, timeOfDayWhereRemind, messageToRemind);
            new OrdersReminder(newReminderID).TryStartReminderThread();
            
            return ReplyAsync(string.Format(REMINDER_WAS_SET_MESSAGE, timeOfDayWhereRemind));
        }
    }
    
    [Group("shall")]
    public class ShowCommandModule : ModuleBase<SocketCommandContext>
    {
        private const string NOTHING_TO_SHOW_MESSAGE = "Nothing to show";
        private const string COMMON_SHOW_COMMAND_NAME = "";
        private const string SORT_SHOW_COMMAND_NAME = "-sort";
        private const string SUM_SHOW_COMMAND_NAME = "-sum";
        
        private OrderDataAsciiTableConverter OrdersOutputMaintainer { get; set; } = new();
        private Dictionary<string, Func<Queue<OrderData>, StringBuilder>> ShowAllOptionsFunctions { get; set; }

        public ShowCommandModule ()
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

            return ReplyAsync(todayOrders.Count == 0 ? NOTHING_TO_SHOW_MESSAGE : ShowAllOptionsFunctions[command]?.Invoke(todayOrders).ToString());
        }
    }
}