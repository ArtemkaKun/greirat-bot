using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string ORDER_WAS_SAVED_MESSAGE = "Order was proceeded";

        [Command("makeOrder")]
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
            Queue<OrderData> todayOrders = DB.Instance.GetTodayOrders();
            
            return ReplyAsync(FormOrdersShowData(todayOrders).ToString());
        }

        [Command("showMyTodayOrders")]
        [Summary("Shows users today's orders")]
        public Task ShowUserTodayOrders ()
        {
            Queue<OrderData> todayOrders = DB.Instance.GetTodayOrders(Context.Message.Author.Username);
            
            return ReplyAsync(FormOrdersShowData(todayOrders).ToString());
        }

        [Command("updateOrder")]
        [Summary("Updates order with provided ID")]
        public Task UpdateUserOrder (int idOfOrder, [Remainder] string newOrderMessage)
        {
            bool updateOperationResult = DB.Instance.TryUpdateOrderData(idOfOrder, newOrderMessage);
            
            return ReplyAsync(updateOperationResult == true ? "Order was successfully updated" : "Failed to update");
        }

        [Command("deleteOrder")]
        [Summary("Deletes order with provided ID")] 
        public Task DeleteOrder (int idOfOrder)
        {
            bool deleteOperationResult = DB.Instance.TryDeleteOrderData(idOfOrder);
            
            return ReplyAsync(deleteOperationResult == true ? "Order was successfully removed" : "Failed to remove");
        }

        private StringBuilder FormOrdersShowData (Queue<OrderData> todayOrders)
        {
            StringBuilder todayOrdersTableBuilder = new();

            while (todayOrders.Count > 0)
            {
                OrderData order = todayOrders.Dequeue();
                todayOrdersTableBuilder.Append($"{order.OrderID.ToString()} {order.PersonName} {order.OrderText} \n");
            }

            return todayOrdersTableBuilder;
        }
    }
}