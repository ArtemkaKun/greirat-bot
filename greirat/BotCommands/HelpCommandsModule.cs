using System.Threading.Tasks;
using Discord.Commands;
using greirat.Helpers;

namespace greirat
{
    public class HelpCommandsModule : ModuleBase<SocketCommandContext>
    {
        private HelpInfoAsciiTableConverter HelpOutputMaintainer { get; set; } = new();
        
        [Command("help")]
        [Alias("h")]
        [Summary("Shows bot's commands")]
        public Task ShowHelpMessage ()
        {
            return ReplyAsync(HelpOutputMaintainer.HelpInfoInTableForm);
        }
    }
}