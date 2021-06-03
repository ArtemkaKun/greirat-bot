using System;
using System.Data.SQLite;

namespace greirat
{
    public class DB
    {
        private const string PATH_TO_DATA_DB_FILE = @"URI=file:data.db";
        private const string NAME_OF_ORDERS_TABLE = "ORDERS";

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
            DateTime todayDay = DateTime.Today;
            CommandExecutor.CommandText = $"INSERT INTO {NAME_OF_ORDERS_TABLE}(date, personName, orderMessage) VALUES('{todayDay.Day.ToString()}-{todayDay.Month.ToString()}-{todayDay.Year.ToString()}','{personName}', '{orderMessage}')";
        }

        private void Initialize ()
        {
            PrepareDBTables();
        }

        private void PrepareDBTables ()
        {
            CommandExecutor.CommandText = $"CREATE TABLE IF NOT EXISTS {NAME_OF_ORDERS_TABLE}(date TEXT NOT NULL PRIMARY KEY, personName TEXT, orderMessage TEXT)";
            CommandExecutor.ExecuteNonQuery();
        }
    }
}