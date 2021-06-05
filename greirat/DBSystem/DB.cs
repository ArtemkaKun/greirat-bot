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
            
            return StoreOrdersDataInQueue(Orders.Where(order => order.OrderDate == todayDate).GetEnumerator());
        }

        public Queue<OrderData> GetTodayOrders (string userName)
        {
            string todayDate = GetTodayDateInStringForm();
            
            return StoreOrdersDataInQueue(Orders.Where(order => (order.PersonName == userName) && (order.OrderDate == todayDate)).GetEnumerator());
        }

        public bool TryUpdateOrderData (int idOfOrder, string newOrderMessage)
        {
            OrderData orderToUpdate = Orders.SingleOrDefault(order => order.OrderID == idOfOrder);
            
            if (orderToUpdate == null)
            {
                return false;
            }

            orderToUpdate.OrderText = newOrderMessage;
            SaveChanges();
            
            return true;
        }

        public bool TryDeleteOrderData (int idOfOrder)
        {
            OrderData orderToUpdate = Orders.SingleOrDefault(order => order.OrderID == idOfOrder);
            
            if (orderToUpdate == null)
            {
                return false;
            }

            Remove(orderToUpdate);
            SaveChanges();

            return true;
        }

        private Queue<OrderData> StoreOrdersDataInQueue (IEnumerator<OrderData> records)
        {
            Queue<OrderData> todayOrders = new ();
            
            while (records.MoveNext() == true)
            {
                todayOrders.Enqueue(records.Current);
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