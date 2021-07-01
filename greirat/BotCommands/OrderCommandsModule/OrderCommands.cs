using System.Threading.Tasks;
using Discord.Commands;
using greirat;

namespace BotCommands
{
    [Group(OrderCommandsModuleDatabase.ORDER_COMMANDS_GROUP_NAME)]
    public class OrderCommands : ModuleBase<SocketCommandContext>
    {
        [Command(CommandsDatabase.MAKE_COMMAND_NAME)]
        [Summary(OrderCommandsModuleDatabase.CREATE_NEW_ORDER_COMMAND_DESCRIPTION)]
        public Task CreateNewOrder ([Remainder] string orderText)
        {
            string operationResult = Program.DBManager.AddNewOrder(Context.Message.Author.Username, orderText);
            return ReplyAsync(operationResult);
        }

        [Command(CommandsDatabase.UPDATE_COMMAND_NAME)]
        [Summary(OrderCommandsModuleDatabase.UPDATE_USER_ORDER_COMMAND_DESCRIPTION)]
        public Task UpdateUserOrder (int idOfOrder, [Remainder] string newOrderText)
        {
            string operationResult = Program.DBManager.TryUpdateOrderData(Context.Message.Author.Username, idOfOrder, newOrderText);
            return ReplyAsync(operationResult);
        }

        [Command(CommandsDatabase.DELETE_COMMAND_NAME)]
        [Summary(OrderCommandsModuleDatabase.DELETE_ORDER_COMMAND_DESCRIPTION)]
        public Task DeleteOrder (int idOfOrder)
        {
            string operationResult = Program.DBManager.TryDeleteOrderData(Context.Message.Author.Username, idOfOrder);
            return ReplyAsync(operationResult);
        }
    }
}