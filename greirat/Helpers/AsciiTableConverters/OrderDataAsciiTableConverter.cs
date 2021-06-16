using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace greirat.Helpers
{
    public class OrderDataAsciiTableConverter : AsciiTableConverter
    {
        private const string ORDER_ID_COLUMN_NAME = "OrderID";
        private const string PERSON_NAME_COLUMN_NAME = "PersonName";
        private const string ORDER_TEXT_COLUMN_NAME = "OrderText";
        
        private DataTable ShowOrderResultsDataTable { get; set; } = new();

        public OrderDataAsciiTableConverter ()
        {
            CreatedOrderResultsDataTableColumns();
        }

        public StringBuilder FormOrdersShowData (Queue<OrderData> todayOrders)
        {
            FillTableWithData(todayOrders);

            return GetAsciiTableBuilder(ShowOrderResultsDataTable);
        }
        
        public StringBuilder FormOrdersSortedShowData (Queue<OrderData> todayOrders)
        {
            using IEnumerator<OrderData> todayOrdersSorted = todayOrders.OrderBy(data => data.OrderText).GetEnumerator();
            FillTableWithData(todayOrdersSorted);

            return GetAsciiTableBuilder(ShowOrderResultsDataTable);
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
        
        private void FillTableWithData (IEnumerator<OrderData> todayOrders)
        {
            ShowOrderResultsDataTable.Clear();

            while (todayOrders.MoveNext() == true)
            {
                CreateTableRowFromOrderData(todayOrders.Current);
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
    }
}