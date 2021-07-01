using System.Threading.Tasks;
using Discord.Commands;
using greirat;

namespace BotCommands
{
    [Group(ORDER_COMMANDS_GROUP_NAME)]
    public class OrderCommands : ModuleBase<SocketCommandContext>
    {
        private const string ORDER_COMMANDS_GROUP_NAME = "order";
        private const string CREATE_NEW_ORDER_COMMAND_DESCRIPTION = "Creates a new order";
        private const string UPDATE_USER_ORDER_COMMAND_DESCRIPTION = "Updates order with provided text";
        private const string DELETE_ORDER_COMMAND_DESCRIPTION = "Deletes order";
        private const string ORDER_WAS_SAVED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "proceeded";
        private const string ORDER_WAS_UPDATED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "updated";
        private const string ORDER_WAS_REMOVED = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "removed";
        private const string ORDER_UPDATE_FAILED = "Failed to update order";
        private const string ORDER_DELETE_FAILED = "Failed to remove order";
        private const string ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE = "Order was successfully ";

        [Command(CommandsDatabase.MAKE_COMMAND_NAME)]
        [Summary(CREATE_NEW_ORDER_COMMAND_DESCRIPTION)]
        public Task CreateNewOrder ([Remainder] string orderText)
        {
            Program.DBManager.AddNewOrder(Context.Message.Author.Username, orderText);

            return ReplyAsync(ORDER_WAS_SAVED_MESSAGE);
        }

        [Command(CommandsDatabase.UPDATE_COMMAND_NAME)]
        [Summary(UPDATE_USER_ORDER_COMMAND_DESCRIPTION)]
        public Task UpdateUserOrder (int idOfOrder, [Remainder] string newOrderText)
        {
            bool updateOperationResult = Program.DBManager.TryUpdateOrderData(Context.Message.Author.Username, idOfOrder, newOrderText);

            return ReplyAsync(updateOperationResult == true ? ORDER_WAS_UPDATED_MESSAGE : ORDER_UPDATE_FAILED);
        }

        [Command(CommandsDatabase.DELETE_COMMAND_NAME)]
        [Summary(DELETE_ORDER_COMMAND_DESCRIPTION)]
        public Task DeleteOrder (int idOfOrder)
        {
            bool deleteOperationResult = Program.DBManager.TryDeleteOrderData(Context.Message.Author.Username, idOfOrder);

            return ReplyAsync(deleteOperationResult == true ? ORDER_WAS_REMOVED : ORDER_DELETE_FAILED);
        }
    }
}