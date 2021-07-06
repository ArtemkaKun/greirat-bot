using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using DBSystem;
using greirat;

namespace Helpers
{
	public class OrderDataAsciiTableConverter : AsciiTableConverter
	{
		private const string ORDER_ID_COLUMN_NAME = "ID";
		private const string PERSON_NAME_COLUMN_NAME = "Person";
		private const string ORDER_TEXT_COLUMN_NAME = "Order";
		private const string ORDERS_COUNT_COLUMN_NAME = "Count";

		private DataTable ShowOrderResultsDataTable { get; set; } = new();
		private DataTable ShowOrderCountsDataTable { get; set; } = new();
		private Dictionary<string, Type> ColumnsNameTypeMap { get; } = new()
		{
			{ORDER_ID_COLUMN_NAME, typeof(int)},
			{PERSON_NAME_COLUMN_NAME, typeof(string)},
			{ORDER_TEXT_COLUMN_NAME, typeof(string)},
			{ORDERS_COUNT_COLUMN_NAME, typeof(int)}
		};

		public OrderDataAsciiTableConverter ()
		{
			CreatedOrderResultsDataTableColumns();
			CreateOrderCountDataTableColumns();
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

		public StringBuilder FormOrdersSummaryShowData (Queue<OrderData> todayOrders)
		{
			using IEnumerator<(string Order, int Count)> todayOrdersSorted = todayOrders.GroupBy(data => data.OrderText).Select(group => (group.Key, group.Count())).GetEnumerator();
			FillTableWithData(todayOrdersSorted);

			return GetAsciiTableBuilder(ShowOrderCountsDataTable);
		}

		private void CreatedOrderResultsDataTableColumns ()
		{
			foreach (string columnName in new[] {ORDER_ID_COLUMN_NAME, PERSON_NAME_COLUMN_NAME, ORDER_TEXT_COLUMN_NAME})
			{
				AddColumnToDataTable(ShowOrderResultsDataTable, columnName);
			}
		}

		private void CreateOrderCountDataTableColumns ()
		{
			foreach (string columnName in new[] {ORDER_TEXT_COLUMN_NAME, ORDERS_COUNT_COLUMN_NAME})
			{
				AddColumnToDataTable(ShowOrderCountsDataTable, columnName);
			}
		}

		private void AddColumnToDataTable (DataTable dataTableToEdit, string columnName)
		{
			dataTableToEdit.Columns.Add(new DataColumn(columnName, ColumnsNameTypeMap[columnName]));
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

		private void FillTableWithData (IEnumerator<(string Order, int Count)> todayOrders)
		{
			ShowOrderCountsDataTable.Clear();

			while (todayOrders.MoveNext() == true)
			{
				CreateTableRowWithOrderCount(todayOrders);
			}
		}

		private void CreateTableRowWithOrderCount (IEnumerator<(string Order, int Count)> todayOrders)
		{
			DataRow tempDataRow = ShowOrderCountsDataTable.NewRow();
			tempDataRow[ORDER_TEXT_COLUMN_NAME] = todayOrders.Current.Order;
			tempDataRow[ORDERS_COUNT_COLUMN_NAME] = todayOrders.Current.Count;
			ShowOrderCountsDataTable.Rows.Add(tempDataRow);
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