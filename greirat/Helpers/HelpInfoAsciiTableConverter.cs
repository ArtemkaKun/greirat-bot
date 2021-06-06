using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Discord.Commands;
using greirat.External;
using ParameterInfo = System.Reflection.ParameterInfo;

namespace greirat.Helpers
{
    public class HelpInfoAsciiTableConverter
    {
        private const string COMMAND_COLUMN_NAME = "Command";
        private const string COMMAND_DESCRIPTION_COLUMN_NAME = "Description";
        private const string DISCORD_FORMATTER_SYMBOLS = "```";
        private const char BOT_COMMAND_PREFIX = '!';

        public StringBuilder HelpInfoInTableForm { get; private set; }
        
        private DataTable ShowHelpResultsDataTable { get; set; } = new();

        public HelpInfoAsciiTableConverter ()
        {
            CreatedOrderResultsDataTableColumns();
            HelpInfoInTableForm = FormOrdersShowData(typeof(CommandsModule).GetMethods().Where(method => method.GetCustomAttribute<CommandAttribute>() != null).ToArray());
        }

        private void CreatedOrderResultsDataTableColumns ()
        {
            ShowHelpResultsDataTable.Columns.Add(new DataColumn(COMMAND_COLUMN_NAME, typeof(string)));
            ShowHelpResultsDataTable.Columns.Add(new DataColumn(COMMAND_DESCRIPTION_COLUMN_NAME, typeof(string)));
        }

        private StringBuilder FormOrdersShowData (MethodInfo[] todayOrders)
        {
            FillTableWithData(todayOrders);

            return GetAsciiTableBuilder();
        }

        private void FillTableWithData (MethodInfo[] todayOrders)
        {
            for (int methodPointer = 0; methodPointer < todayOrders.Length; methodPointer++)
            {
                CreateTableRowFromOrderData(todayOrders[methodPointer]);
            }
        }

        private void CreateTableRowFromOrderData (MethodInfo order)
        {
            DataRow tempDataRow = ShowHelpResultsDataTable.NewRow();
            tempDataRow[COMMAND_COLUMN_NAME] = $"{BOT_COMMAND_PREFIX}{order.GetCustomAttribute<CommandAttribute>()?.Text}{GetMethodArgumentNames(order)}";
            tempDataRow[COMMAND_DESCRIPTION_COLUMN_NAME] = order.GetCustomAttribute<SummaryAttribute>()?.Text ?? string.Empty;
            ShowHelpResultsDataTable.Rows.Add(tempDataRow);
        }

        private string GetMethodArgumentNames (MethodInfo order)
        {
            StringBuilder argumentsNamesStringBuilder = new();

            foreach (ParameterInfo argument in order.GetParameters())
            {
                argumentsNamesStringBuilder.Append($" <{argument.Name}>");
            }
            
            return argumentsNamesStringBuilder.ToString();
        }

        private StringBuilder GetAsciiTableBuilder ()
        {
            StringBuilder asciiTableBuilder = AsciiTableGenerator.CreateAsciiTableFromDataTable(ShowHelpResultsDataTable);
            PrepareBuilderToDiscordFormatting(asciiTableBuilder);

            return asciiTableBuilder;
        }

        private static void PrepareBuilderToDiscordFormatting (StringBuilder asciiTableBuilder)
        {
            asciiTableBuilder.Insert(0, DISCORD_FORMATTER_SYMBOLS);
            asciiTableBuilder.Append(DISCORD_FORMATTER_SYMBOLS);
        }
    }
}