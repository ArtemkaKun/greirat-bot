using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    public class HelpCommandsModule : ModuleBase<SocketCommandContext>
    {
        [Command("help")]
        [Alias("h")]
        [Summary("Shows bot's commands")]
        public Task ShowHelpMessage ()
        {
            return ReplyAsync(Program.HelpOutputMaintainer.HelpInfoInTableForm);
        }
    }
}