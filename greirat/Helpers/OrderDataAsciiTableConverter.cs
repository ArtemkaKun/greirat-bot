using System.Collections.Generic;
using System.Data;
using System.Text;
using greirat.External;

namespace greirat.Helpers
{
    public class OrderDataAsciiTableConverter
    {
        private const string ORDER_ID_COLUMN_NAME = "OrderID";
        private const string PERSON_NAME_COLUMN_NAME = "PersonName";
        private const string ORDER_TEXT_COLUMN_NAME = "OrderText";
        private const string DISCORD_FORMATTER_SYMBOLS = "```";

        private DataTable ShowOrderResultsDataTable { get; set; } = new();

        public OrderDataAsciiTableConverter ()
        {
            CreatedOrderResultsDataTableColumns();
        }

        public StringBuilder FormOrdersShowData (Queue<OrderData> todayOrders)
        {
            FillTableWithData(todayOrders);

            return GetAsciiTableBuilder();
        }

        private void CreatedOrderResultsDataTableColumns ()
        {
            ShowOrderResultsDataTable.Columns.Add(new DataColumn(ORDER_ID_COLUMN_NAME, typeof(int)));
            ShowOrderResultsDataTable.Columns.Add(new DataColumn(PERSON_NAME_COLUMN_NAME, typeof(string)));
            ShowOrderResultsDataTable.Columns.Add(new DataColumn(ORDER_TEXT_COLUMN_NAME, typeof(string)));
        }

        private void FillTableWithData (Queue<OrderData> todayOrders)
        {
            ShowOrderResultsDataTable.Clear();

            while (todayOrders.Count > 0)
            {
                CreateTableRowFromOrderData(todayOrders.Dequeue());
            }
        }

        private void CreateTableRowFromOrderData (OrderData order)
        {
            DataRow tempDataRow = ShowOrderResultsDataTable.NewRow();
            tempDataRow[ORDER_ID_COLUMN_NAME] = order.OrderID;
            tempDataRow[PERSON_NAME_COLUMN_NAME] = order.PersonName;
            tempDataRow[ORDER_TEXT_COLUMN_NAME] = order.OrderText;
            ShowOrderResultsDataTable.Rows.Add(tempDataRow);
        }

        private StringBuilder GetAsciiTableBuilder ()
        {
            StringBuilder asciiTableBuilder = AsciiTableGenerator.CreateAsciiTableFromDataTable(ShowOrderResultsDataTable);
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