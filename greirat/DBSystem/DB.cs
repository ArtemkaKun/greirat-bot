using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BotCommands;
using Discord.Commands;
using greirat;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.ChangeTracking;
using VoteReminderSystem;

namespace DBSystem
{
	public class DB : DbContext
	{
		private const string PATH_TO_DATA_DB_FILE = @"Data Source=data.db";
		private const string TODAY_DATA_STRING_TEMPLATE = "{0}-{1}-{2}";

		private DbSet<OrderData> Orders { get; set; }
		private DbSet<VoteRemindData> RemindersData { get; set; }

		protected override void OnConfiguring (DbContextOptionsBuilder options)
		{
			options.UseSqlite(PATH_TO_DATA_DB_FILE);
		}

		public void EnsureDBIsCreated ()
		{
			Database.EnsureCreated();
		}

		public string AddNewOrder (SimpleOrderInfo orderInfo)
		{
			OrderData newOrder = new()
			{
				Day = DateTime.Today,
				OwnerName = orderInfo.OrderOwner,
				Text = orderInfo.OrderMessage
			};

			Add(newOrder);
			SaveChanges();

			return OrderCommandsModuleDatabase.ORDER_WAS_SAVED_MESSAGE;
		}

		public Queue<OrderData> GetTodayOrders ()
		{
			return StoreOrdersDataInQueue(GetTodayOrdersEnumerator());
		}

		public Queue<OrderData> GetTodayOrders (string userName)
		{
			return StoreOrdersDataInQueue(GetTodayOrdersEnumerator(order => order.OwnerName == userName));
		}

		public string TryUpdateOrderData (SimpleOrderInfo orderInfo)
		{
			OrderData orderToUpdate = FindOrder(orderInfo.OrderOwner, orderInfo.OrderID);

			if (orderToUpdate == null)
			{
				return OrderCommandsModuleDatabase.ORDER_UPDATE_FAILED;
			}

			OrderData updatedOrder = new()
			{
				Day = DateTime.Today,
				OwnerName = orderInfo.OrderOwner,
				Text = orderInfo.OrderMessage
			};

			Remove(orderToUpdate);
			Add(updatedOrder);
			SaveChanges();

			return OrderCommandsModuleDatabase.ORDER_WAS_UPDATED_MESSAGE;
		}

		public string TryDeleteOrderData (SimpleOrderInfo orderInfo)
		{
			OrderData orderToUpdate = FindOrder(orderInfo.OrderOwner, orderInfo.OrderID);

			if (orderToUpdate == null)
			{
				return OrderCommandsModuleDatabase.ORDER_DELETE_FAILED;
			}

			Remove(orderToUpdate);
			SaveChanges();

			return OrderCommandsModuleDatabase.ORDER_WAS_REMOVED;
		}

		public VoteRemindData AddNewReminder (SocketCommandContext messageData, SimpleReminderInfo reminderInfo)
		{
			EntityEntry<VoteRemindData> createdReminder = Add(new VoteRemindData(messageData, reminderInfo));
			SaveChanges();
			return new VoteRemindData(createdReminder.Entity);
		}

		public Stack<VoteRemindData> GetAllRemindersFromDB ()
		{
			return new(RemindersData);
		}

		public void DeleteReminder (VoteRemindData reminderDataToDelete)
		{
			Remove(reminderDataToDelete);
			SaveChanges();
		}

		public void UpdateReminder (VoteRemindData reminderToUpdate)
		{
			VoteRemindData reminder = RemindersData.SingleOrDefault(reminderData => reminderData.ReminderID == reminderToUpdate.ReminderID);

			if (reminder == null)
			{
				return;
			}

			reminder.UpdateReminderData(reminderToUpdate);
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
			if (additionalCheckExpression == null)
			{
				return Orders.Where(order => order.Day == DateTime.Today).GetEnumerator();
			}

			return Orders.Where(order => order.Day == DateTime.Today).Where(additionalCheckExpression).GetEnumerator();
		}

		private OrderData FindOrder (string requestFromUsername, int idOfOrder)
		{
			return Orders.SingleOrDefault(order => (order.OwnerName == requestFromUsername) && (order.ID == idOfOrder));
		}
	}
}