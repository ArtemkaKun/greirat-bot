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
            CreatedCommandsDataTableColumns();
            MethodInfo[] commandsMethods = GetAllCommandsMethods();
            HelpInfoInTableForm = FormCommandsShowData(commandsMethods).ToString();
        }

        private void CreatedCommandsDataTableColumns ()
        {
            ShowHelpResultsDataTable.Columns.Add(new DataColumn(COMMAND_COLUMN_NAME, typeof(string)));
            ShowHelpResultsDataTable.Columns.Add(new DataColumn(COMMAND_DESCRIPTION_COLUMN_NAME, typeof(string)));
        }

        private MethodInfo[] GetAllCommandsMethods ()
        {
            return Assembly.GetEntryAssembly()?.GetTypes().Where(typeUnit => typeUnit.IsAssignableTo(typeof(ModuleBase<SocketCommandContext>)))
                           .SelectMany(commandsModule => commandsModule.GetMethods().Where(method => method.GetCustomAttribute<CommandAttribute>() != null))
                           .ToArray();
        }

        private StringBuilder FormCommandsShowData (MethodInfo[] commandMethods)
        {
            FillTableWithData(commandMethods);

            return GetAsciiTableBuilder(ShowHelpResultsDataTable);
        }

        private void FillTableWithData (MethodInfo[] commandMethods)
        {
            for (int methodPointer = 0; methodPointer < commandMethods.Length; methodPointer++)
            {
                CreateTableRowFromOrderData(commandMethods[methodPointer]);
            }
        }

        private void CreateTableRowFromOrderData (MethodInfo command)
        {
            DataRow tempDataRow = ShowHelpResultsDataTable.NewRow();
            tempDataRow[COMMAND_COLUMN_NAME] = $"{GetCommandNameWithAliases(command)}{GetMethodArgumentNames(command)}";
            tempDataRow[COMMAND_DESCRIPTION_COLUMN_NAME] = command.GetCustomAttribute<SummaryAttribute>()?.Text ?? string.Empty;
            ShowHelpResultsDataTable.Rows.Add(tempDataRow);
        }

        private string GetCommandNameWithAliases (MethodInfo command)
        {
            CommandNamesBuilder.Clear();
            AppendCommandName(GetCommandName(command));
            AppendAllAliases(command);

            return CommandNamesBuilder.ToString();
        }

        private string GetCommandName (MethodInfo command)
        {
            string commandsGroupName = command.DeclaringType?.GetCustomAttribute<GroupAttribute>()?.Prefix;
            string commandMethodName = command.GetCustomAttribute<CommandAttribute>()?.Text;
            
            return string.IsNullOrEmpty(commandsGroupName) == false ? $"{commandsGroupName} {commandMethodName}" : commandMethodName;
        }

        private void AppendAllAliases (MethodInfo command)
        {
            string[] commandAliases = command.GetCustomAttribute<AliasAttribute>()?.Aliases;

            for (int aliasPointer = 0; aliasPointer < commandAliases?.Length; aliasPointer++)
            {
                AppendCommandName(commandAliases[aliasPointer]);
            }
        }

        private void AppendCommandName (string commandName)
        {
            if (CommandNamesBuilder.Length > 0)
            {
                CommandNamesBuilder.Append(TEXT_BETWEEN_COMMAND_NAMES);
            }

            CommandNamesBuilder.AppendFormat(COMMAND_NAME_TEMPLATE, commandName);
        }

        private string GetMethodArgumentNames (MethodInfo command)
        {
            StringBuilder argumentsNamesStringBuilder = new();

            foreach (ParameterInfo argument in command.GetParameters())
            {
                argumentsNamesStringBuilder.Append(string.Format(COMMAND_ARGUMENT_NAME_TEMPLATE, argument.Name));
            }

            return argumentsNamesStringBuilder.ToString();
        }
    }
}