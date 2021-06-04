using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.Linq;
using System.Reflection;
using Microsoft.EntityFrameworkCore;

namespace greirat
{
    public class DB : DbContext
    {
        private const string PATH_TO_DATA_DB_FILE = @"Data Source=data.db";
        private const string TODAY_DATA_STRING_TEMPLATE = "{0}-{1}-{2}";

        public static DB Instance { get; private set; } = new ();
        private DbSet<OrderData> Orders { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder options)
        {
            options.UseSqlite(PATH_TO_DATA_DB_FILE);
        }

        public void AddNewOrder (string personName, string orderMessage)
        {
            Database.EnsureCreated();
            Add(new OrderData(GetTodayDateInStringForm(), personName, orderMessage));
            SaveChanges();
        }

        public Queue<OrderData> GetTodayOrders ()
        {
            string todayDate = GetTodayDateInStringForm();
            using IEnumerator<OrderData> todayRecords = Orders.Where(order => order.OrderDate == todayDate).GetEnumerator();
            Queue<OrderData> todayOrders = new ();
            
            while (todayRecords.MoveNext() == true)
            {
                todayOrders.Enqueue(todayRecords.Current);
            }

            return todayOrders;
        }

        private string GetTodayDateInStringForm ()
        {
            DateTime todayDay = DateTime.Today;
            return string.Format(TODAY_DATA_STRING_TEMPLATE, todayDay.Day.ToString(), todayDay.Month.ToString(), todayDay.Year.ToString());
        }
    }
}