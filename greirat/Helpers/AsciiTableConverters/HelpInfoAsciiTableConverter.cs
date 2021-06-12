using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;
using Discord.Commands;
using ParameterInfo = System.Reflection.ParameterInfo;

namespace greirat.Helpers
{
    public class HelpInfoAsciiTableConverter : AsciiTableConverter
    {
        private const string COMMAND_COLUMN_NAME = "Command";
        private const string COMMAND_DESCRIPTION_COLUMN_NAME = "Description";
        private const string COMMAND_NAME_TEMPLATE = "!{0}";
        private const string COMMAND_ARGUMENT_NAME_TEMPLATE = " <{0}>";
        private const string TEXT_BETWEEN_COMMAND_NAMES = ", ";

        public string HelpInfoInTableForm { get; private set; }

        private DataTable ShowHelpResultsDataTable { get; set; } = new();
        private StringBuilder CommandNamesBuilder { get; set; } = new();

        public HelpInfoAsciiTableConverter ()
        {
            CreatedOrderResultsDataTableColumns();
            HelpInfoInTableForm = FormOrdersShowData(typeof(CommandsModule).GetMethods().Where(method => method.GetCustomAttribute<CommandAttribute>() != null).ToArray()).ToString();
        }

        private void CreatedOrderResultsDataTableColumns ()
        {
            ShowHelpResultsDataTable.Columns.Add(new DataColumn(COMMAND_COLUMN_NAME, typeof(string)));
            ShowHelpResultsDataTable.Columns.Add(new DataColumn(COMMAND_DESCRIPTION_COLUMN_NAME, typeof(string)));
        }

        private StringBuilder FormOrdersShowData (MethodInfo[] todayOrders)
        {
            FillTableWithData(todayOrders);

            return GetAsciiTableBuilder(ShowHelpResultsDataTable);
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
            tempDataRow[COMMAND_COLUMN_NAME] = $"{GetCommandNameWithAliases(order)}{GetMethodArgumentNames(order)}";
            tempDataRow[COMMAND_DESCRIPTION_COLUMN_NAME] = order.GetCustomAttribute<SummaryAttribute>()?.Text ?? string.Empty;
            ShowHelpResultsDataTable.Rows.Add(tempDataRow);
        }

        private string GetCommandNameWithAliases (MethodInfo order)
        {
            CommandNamesBuilder.Clear();
            AppendCommandName(order.GetCustomAttribute<CommandAttribute>()?.Text);
            string[] commandAliases = order.GetCustomAttribute<AliasAttribute>()?.Aliases;

            for (int aliasPointer = 0; aliasPointer < commandAliases?.Length; aliasPointer++)
            {
                AppendCommandName(commandAliases[aliasPointer]);
            }

            return CommandNamesBuilder.ToString();
        }

        private void AppendCommandName (string commandName)
        {
            if (CommandNamesBuilder.Length > 0)
            {
                CommandNamesBuilder.Append(TEXT_BETWEEN_COMMAND_NAMES);
            }

            CommandNamesBuilder.AppendFormat(COMMAND_NAME_TEMPLATE, commandName);
        }

        private string GetMethodArgumentNames (MethodInfo order)
        {
            StringBuilder argumentsNamesStringBuilder = new();

            foreach (ParameterInfo argument in order.GetParameters())
            {
                argumentsNamesStringBuilder.Append(string.Format(COMMAND_ARGUMENT_NAME_TEMPLATE, argument.Name));
            }

            return argumentsNamesStringBuilder.ToString();
        }
    }
}