using System;
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
			return ProceedOrderCommandWithReply(Program.DBManager.AddNewOrder, new OrderInfo
			{
				Text = orderText,
				OwnerName = GetOrderOwnerName()
			});
		}

		[Command(CommandsDatabase.UPDATE_COMMAND_NAME)]
		[Summary(OrderCommandsModuleDatabase.UPDATE_USER_ORDER_COMMAND_DESCRIPTION)]
		public Task UpdateUserOrder (int idOfOrder, [Remainder] string newOrderText)
		{
			return ProceedOrderCommandWithReply(Program.DBManager.TryUpdateOrderData, new OrderInfo
			{
				ID = idOfOrder,
				Text = newOrderText,
				OwnerName = GetOrderOwnerName()
			});
		}

		[Command(CommandsDatabase.DELETE_COMMAND_NAME)]
		[Summary(OrderCommandsModuleDatabase.DELETE_ORDER_COMMAND_DESCRIPTION)]
		public Task DeleteOrder (int idOfOrder)
		{
			return ProceedOrderCommandWithReply(Program.DBManager.TryDeleteOrderData, new OrderInfo
			{
				ID = idOfOrder,
				OwnerName = GetOrderOwnerName()
			});
		}

		private Task ProceedOrderCommandWithReply (Func<OrderInfo, string> actionToPerform, OrderInfo orderInfo)
		{
			string operationResult = actionToPerform.Invoke(orderInfo);
			return ReplyAsync(operationResult);
		}

		private string GetOrderOwnerName ()
		{
			return Context.Message.Author.Username;
		}
	}
}