using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace greirat
{
    public class DB
    {
        private const string PATH_TO_DATA_DB_FILE = @"URI=file:data.db";
        private const string NAME_OF_ORDERS_TABLE = "ORDERS";
        private const string NAME_OF_THE_ID_COLUMN = "ID";
        private const string NAME_OF_DATE_COLUMN = "date";
        private const string NAME_OF_PERSON_NAME_COLUMN = "personName";
        private const string NAME_OF_ORDER_TEXT_COLUMN = "orderMessage";
        private const string TODAY_DATA_STRING_TEMPLATE = "{0}-{1}-{2}";

        public static DB Instance { get; private set; } = new ();

        private SQLiteConnection DBConnection { get; set; } = new(PATH_TO_DATA_DB_FILE);
        private SQLiteCommand CommandExecutor { get; set; }

        private DB ()
        {
            DBConnection.Open();
            CommandExecutor = new SQLiteCommand(DBConnection);
            Initialize();
        }

        public void AddNewOrder (string personName, string orderMessage)
        {
            CommandExecutor.CommandText = $"INSERT INTO {NAME_OF_ORDERS_TABLE}({NAME_OF_DATE_COLUMN}, {NAME_OF_PERSON_NAME_COLUMN}, {NAME_OF_ORDER_TEXT_COLUMN}) VALUES('{GetTodayDateInStringForm()}','{personName}', '{orderMessage}')";
            CommandExecutor.ExecuteNonQuery();
        }

        public Stack<OrderData> GetTodayOrders ()
        {
            CommandExecutor.CommandText = $"SELECT {NAME_OF_THE_ID_COLUMN},{NAME_OF_PERSON_NAME_COLUMN},{NAME_OF_ORDER_TEXT_COLUMN} FROM {NAME_OF_ORDERS_TABLE} WHERE {NAME_OF_DATE_COLUMN}='{GetTodayDateInStringForm()}'";
            using SQLiteDataReader executeReader = CommandExecutor.ExecuteReader();

            Stack<OrderData> todayOrders = new ();
            
            while (executeReader.Read())
            {
                todayOrders.Push(new OrderData(executeReader.GetInt32(0), executeReader.GetString(1), executeReader.GetString(2)));
            }

            return todayOrders;
        }

        private void Initialize ()
        {
            PrepareDBTables();
        }

        private void PrepareDBTables ()
        {
            CommandExecutor.CommandText = $"CREATE TABLE IF NOT EXISTS {NAME_OF_ORDERS_TABLE}({NAME_OF_THE_ID_COLUMN} INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, {NAME_OF_DATE_COLUMN} TEXT NOT NULL, {NAME_OF_PERSON_NAME_COLUMN} TEXT, {NAME_OF_ORDER_TEXT_COLUMN} TEXT)";
            CommandExecutor.ExecuteNonQuery();
        }

        private string GetTodayDateInStringForm ()
        {
            DateTime todayDay = DateTime.Today;
            return string.Format(TODAY_DATA_STRING_TEMPLATE, todayDay.Day.ToString(), todayDay.Month.ToString(), todayDay.Year.ToString());
        }
    }
}