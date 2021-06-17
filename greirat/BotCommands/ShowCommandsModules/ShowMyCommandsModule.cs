using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group("shmy")]
    public class ShowMyCommandsModule : BaseShowCommandsModule
    {
        [Command(COMMON_SHOW_COMMAND_NAME)]
        [Summary("Shows your today's orders")]
        public Task ShowUserTodayOrders ()
        {
            return ShowOrdersData(COMMON_SHOW_COMMAND_NAME, GetAllUserTodayOrders());
        }

        private Queue<OrderData> GetAllUserTodayOrders ()
        {
            return Program.DBManager.GetTodayOrders(Context.Message.Author.Username);
        }
    }
}