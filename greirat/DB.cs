using System.Data.SQLite;

namespace greirat
{
    public class DB
    {
        private const string PATH_TO_DATA_DB_FILE = @"URI=file:data.db";
        private const string NAME_OF_ORDERS_TABLE = "ORDERS";

        private SQLiteConnection DBConnection { get; set; } = new(PATH_TO_DATA_DB_FILE);
        private SQLiteCommand CommandExecutor { get; set; }

        public DB ()
        {
            DBConnection.Open();
            CommandExecutor = new SQLiteCommand(DBConnection);
        }

        public void Initialize ()
        {
            PrepareDBTables();
        }

        private void PrepareDBTables ()
        {
            CommandExecutor.CommandText = $"CREATE TABLE IF NOT EXISTS {NAME_OF_ORDERS_TABLE}(date VARCHAR(256) NOT NULL PRIMARY KEY, personName VARCHAR(512), orderMessage VARCHAR(1024))";
            CommandExecutor.ExecuteNonQuery();
        }
    }
}