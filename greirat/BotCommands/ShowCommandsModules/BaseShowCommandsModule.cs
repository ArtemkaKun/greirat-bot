using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord.Commands;
using greirat;

namespace BotCommands
{
	public abstract class BaseShowCommandsModule : ModuleBase<SocketCommandContext>
	{
		protected const string COMMON_SHOW_COMMAND_NAME = "";
		protected const string SORT_SHOW_COMMAND_NAME = "-sort";
		protected const string SUM_SHOW_COMMAND_NAME = "-sum";

		private Dictionary<string, Func<Queue<OrderData>, StringBuilder>> ShowAllOptionsFunctions { get; set; }

		protected BaseShowCommandsModule ()
		{
			ShowAllOptionsFunctions = new Dictionary<string, Func<Queue<OrderData>, StringBuilder>>
			{
				{COMMON_SHOW_COMMAND_NAME, Program.OrdersOutputMaintainer.FormOrdersShowData},
				{SORT_SHOW_COMMAND_NAME, Program.OrdersOutputMaintainer.FormOrdersSortedShowData},
				{SUM_SHOW_COMMAND_NAME, Program.OrdersOutputMaintainer.FormOrdersSummaryShowData}
			};
		}

		protected Task ShowOrdersData (string command, Queue<OrderData> todayOrders)
		{
			return ReplyAsync(todayOrders.Count == 0 ? CommandsDatabase.NOTHING_TO_SHOW_MESSAGE : ShowAllOptionsFunctions[command]?.Invoke(todayOrders).ToString());
		}
	}
}