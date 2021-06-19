using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;

namespace greirat
{
    public class DB : DbContext
    {
        private const string PATH_TO_DATA_DB_FILE = @"Data Source=data.db";
        private const string TODAY_DATA_STRING_TEMPLATE = "{0}-{1}-{2}";

        private DbSet<OrderData> Orders { get; set; }
        private DbSet<FoodRemindData> RemindersData { get; set; }

        protected override void OnConfiguring (DbContextOptionsBuilder options)
        {
            options.UseSqlite(PATH_TO_DATA_DB_FILE);
        }

        public void EnsureThatDBIsCreated ()
        {
            Database.EnsureCreated();
        }

        public void AddNewOrder (string personName, string orderMessage)
        {
            Add(new OrderData(GetTodayDateInStringForm(), personName, orderMessage));
            SaveChanges();
        }

        public Queue<OrderData> GetTodayOrders ()
        {
            return StoreOrdersDataInQueue(GetTodayOrdersEnumerator());
        }

        public Queue<OrderData> GetTodayOrders (string userName)
        {
            return StoreOrdersDataInQueue(GetTodayOrdersEnumerator(order => order.PersonName == userName));
        }

        public bool TryUpdateOrderData (string requestFromUsername, int idOfOrder, string newOrderMessage)
        {
            OrderData orderToUpdate = FindOrder(requestFromUsername, idOfOrder);

            if (orderToUpdate == null)
            {
                return false;
            }

            orderToUpdate.OrderText = newOrderMessage;
            SaveChanges();

            return true;
        }

        public bool TryDeleteOrderData (string requestFromUsername, int idOfOrder)
        {
            OrderData orderToUpdate = FindOrder(requestFromUsername, idOfOrder);

            if (orderToUpdate == null)
            {
                return false;
            }

            Remove(orderToUpdate);
            SaveChanges();

            return true;
        }

        public FoodRemindData AddNewReminder (SocketCommandContext messageData, string timeToRemind, string messageToRemind)
        {
            EntityEntry<FoodRemindData> createdReminder = Add(new FoodRemindData(messageData.Guild.Id, messageData.Message.Channel.Id, timeToRemind, messageToRemind));
            SaveChanges();
            return createdReminder.Entity;
        }

        public Stack<FoodRemindData> GetAllRemindersFromDB ()
        {
            return new(RemindersData);
        }

        public void DeleteReminder (FoodRemindData reminderDataToDelete)
        {
            Remove(reminderDataToDelete);
            SaveChanges();
        }
        
        private Queue<OrderData> StoreOrdersDataInQueue (IEnumerator<OrderData> records)
        {
            Queue<OrderData> todayOrders = new();

            while (records.MoveNext() == true)
            {
                todayOrders.Enqueue(records.Current);
            }

            return todayOrders;
        }

        private IEnumerator<OrderData> GetTodayOrdersEnumerator (Expression<Func<OrderData, bool>> additionalCheckExpression = null)
        {
            string todayDate = GetTodayDateInStringForm();

            if (additionalCheckExpression == null)
            {
                return Orders.Where(order => order.OrderDate == todayDate).GetEnumerator();
            }

            return Orders.Where(order => order.OrderDate == todayDate).Where(additionalCheckExpression).GetEnumerator();
        }

        private string GetTodayDateInStringForm ()
        {
            DateTime todayDay = DateTime.Today;
            return string.Format(TODAY_DATA_STRING_TEMPLATE, todayDay.Day.ToString(), todayDay.Month.ToString(), todayDay.Year.ToString());
        }

        private OrderData FindOrder (string requestFromUsername, int idOfOrder)
        {
            return Orders.SingleOrDefault(order => (order.PersonName == requestFromUsername) && (order.OrderID == idOfOrder));
        }
    }
}