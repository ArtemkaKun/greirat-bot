using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string ORDER_WAS_SAVED_MESSAGE = "Order was proceeded";

        [Command("order+")]
        [Summary("Creates new order")]
        public Task CreateNewOrder ([Remainder] string orderText)
        {
            DB.Instance.AddNewOrder(Context.Message.Author.Username, orderText);
            return ReplyAsync(ORDER_WAS_SAVED_MESSAGE);
        }
        
        [Command("showTodayOrders")]
        [Summary("Shows today's orders")]
        public Task ShowTodayOrders ()
        {
            Stack<OrderData> todayOrders = DB.Instance.GetTodayOrders();
            StringBuilder todayOrdersTableBuilder = new();

            while (todayOrders.Count > 0)
            {
                OrderData order = todayOrders.Pop();
                todayOrdersTableBuilder.Append($"{order.PersonName} {order.OrderText} \n");
            }
            
            return ReplyAsync(todayOrdersTableBuilder.ToString());
        }
    }
}