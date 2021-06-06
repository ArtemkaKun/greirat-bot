using System.Data;
using System.Text;
using greirat.External;

namespace greirat.Helpers
{
    public class AsciiTableConverter
    {
        private const string DISCORD_FORMATTER_SYMBOLS = "```";

        protected StringBuilder GetAsciiTableBuilder (DataTable dataInTableForm)
        {
            StringBuilder asciiTableBuilder = AsciiTableGenerator.CreateAsciiTableFromDataTable(dataInTableForm);
            PrepareBuilderToDiscordFormatting(asciiTableBuilder);

            return asciiTableBuilder;
        }

        private void PrepareBuilderToDiscordFormatting (StringBuilder asciiTableBuilder)
        {
            asciiTableBuilder.Insert(0, DISCORD_FORMATTER_SYMBOLS);
            asciiTableBuilder.Append(DISCORD_FORMATTER_SYMBOLS);
        }
    }
}