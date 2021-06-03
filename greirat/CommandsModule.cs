using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string ORDER_WAS_SAVED_MESSAGE = "Order was proceeded";

        [Command("order+")]
        [Summary("Creates new order")]
        public Task CreateNewOrder (string orderText)
        {
            DB.Instance.AddNewOrder(Context.Message.Author.Username, orderText);
            return ReplyAsync(ORDER_WAS_SAVED_MESSAGE);
        }
    }
}