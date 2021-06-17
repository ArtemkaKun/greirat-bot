using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("order")]
    public class OrderCommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string ORDER_WAS_SAVED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "proceeded";
        private const string ORDER_WAS_UPDATED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "updated";
        private const string ORDER_WAS_REMOVED = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "removed";
        private const string ORDER_UPDATE_FAILED = "Failed to update order";
        private const string ORDER_DELETE_FAILED = "Failed to remove order";
        private const string ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE = "Order was successfully ";

        [Command("-mk")]
        [Summary("Creates a new order")]
        public Task CreateNewOrder ([Remainder] string orderText)
        {
            Program.DBManager.AddNewOrder(Context.Message.Author.Username, orderText);

            return ReplyAsync(ORDER_WAS_SAVED_MESSAGE);
        }

        [Command("-upd")]
        [Summary("Updates order with provided text.")]
        public Task UpdateUserOrder (int idOfOrder, [Remainder] string newOrderText)
        {
            bool updateOperationResult = Program.DBManager.TryUpdateOrderData(Context.Message.Author.Username, idOfOrder, newOrderText);

            return ReplyAsync(updateOperationResult == true ? ORDER_WAS_UPDATED_MESSAGE : ORDER_UPDATE_FAILED);
        }

        [Command("-del")]
        [Summary("Deletes order.")]
        public Task DeleteOrder (int idOfOrder)
        {
            bool deleteOperationResult = Program.DBManager.TryDeleteOrderData(Context.Message.Author.Username, idOfOrder);

            return ReplyAsync(deleteOperationResult == true ? ORDER_WAS_REMOVED : ORDER_DELETE_FAILED);
        }
    }
}