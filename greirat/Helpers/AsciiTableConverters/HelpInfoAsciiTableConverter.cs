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
        private const string COMMAND_NAME_TEMPLATE = "!{0}{1}";
        private const string COMMAND_ARGUMENT_NAME_TEMPLATE = " <{0}>";

        public string HelpInfoInTableForm { get; private set; }
        
        private DataTable ShowHelpResultsDataTable { get; set; } = new();

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
            tempDataRow[COMMAND_COLUMN_NAME] = string.Format(COMMAND_NAME_TEMPLATE, order.GetCustomAttribute<CommandAttribute>()?.Text, GetMethodArgumentNames(order));
            tempDataRow[COMMAND_DESCRIPTION_COLUMN_NAME] = order.GetCustomAttribute<SummaryAttribute>()?.Text ?? string.Empty;
            ShowHelpResultsDataTable.Rows.Add(tempDataRow);
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