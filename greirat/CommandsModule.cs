using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        [Command("say")]
        [Summary("Echoes a message.")]
        public Task SayAsync([Remainder] [Summary("The text to echo")] string echo)
            => ReplyAsync(echo);
    }
}