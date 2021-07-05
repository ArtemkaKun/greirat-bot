using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using greirat;

namespace BotCommands
{
	[Group(ShowCommandsModuleDatabase.SHOW_USER_COMMANDS_GROUP_NAME)]
	public class ShowMyCommandsModule : BaseShowCommandsModule
	{
		[Command(COMMON_SHOW_COMMAND_NAME)]
		[Summary(ShowCommandsModuleDatabase.SHOW_USER_TODAY_ORDERS_COMMAND_DESCRIPTION)]
		public Task ShowUserTodayOrders ()
		{
			return ShowOrdersData(COMMON_SHOW_COMMAND_NAME, GetAllUserTodayOrders());
		}

		[Command(SORT_SHOW_COMMAND_NAME)]
		[Summary(ShowCommandsModuleDatabase.SHOW_USER_TODAY_ORDERS_SORTED_COMMAND_DESCRIPTION)]
		public Task ShowUserTodayOrdersSorted ()
		{
			return ShowOrdersData(SORT_SHOW_COMMAND_NAME, GetAllUserTodayOrders());
		}

		[Command(SUM_SHOW_COMMAND_NAME)]
		[Summary(ShowCommandsModuleDatabase.SHOW_USER_TODAY_ORDERS_SUMMARY_COMMAND_DESCRIPTION)]
		public Task ShowUserTodayOrdersSummary ()
		{
			return ShowOrdersData(SUM_SHOW_COMMAND_NAME, GetAllUserTodayOrders());
		}

		private Queue<OrderData> GetAllUserTodayOrders ()
		{
			return Program.DBManager.GetTodayOrders(Context.Message.Author.Username);
		}
	}
}