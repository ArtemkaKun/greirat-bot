using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using BotCommands;
using Discord.Commands;
using Microsoft.EntityFrameworkCore;
using VoteReminderSystem;

namespace DBSystem;

public class DB : DbContext
{
	private const string PATH_TO_DATA_DB_FILE = @"Data Source=data.db";

	private DbSet<OrderData> Orders { get; set; } = null!;
	private DbSet<VoteData> Votes { get; set; } = null!;

	protected override void OnConfiguring (DbContextOptionsBuilder options)
	{
		options.UseSqlite(PATH_TO_DATA_DB_FILE);
	}

	public void EnsureDBIsCreated ()
	{
		Database.EnsureCreated();
	}

	public string AddNewOrder (OrderInfo orderInfo)
	{
		OrderData newOrder = CreateNewOrderFromInfo(orderInfo);
		Add(newOrder);
		SaveChanges();

		return OrderCommandsModuleDatabase.ORDER_WAS_SAVED_MESSAGE;
	}

	public string TryUpdateOrderData (OrderInfo orderInfo)
	{
		OrderData? orderToUpdate = FindOrder(orderInfo);

		if (orderToUpdate == null)
		{
			return OrderCommandsModuleDatabase.ORDER_UPDATE_FAILED;
		}

		OrderData newOrder = CreateNewOrderFromInfo(orderInfo);
		Remove(orderToUpdate);
		Add(newOrder);
		SaveChanges();

		return OrderCommandsModuleDatabase.ORDER_WAS_UPDATED_MESSAGE;
	}

	private OrderData CreateNewOrderFromInfo (OrderInfo orderInfo)
	{
		OrderData newOrder = new()
		{
			Day = DateTime.Today,
			OwnerName = orderInfo.OwnerName,
			Text = orderInfo.Text ?? ""
		};

		return newOrder;
	}

	public string TryDeleteOrderData (OrderInfo orderInfo)
	{
		OrderData? orderToDelete = FindOrder(orderInfo);

		if (orderToDelete == null)
		{
			return OrderCommandsModuleDatabase.ORDER_DELETE_FAILED;
		}

		Remove(orderToDelete);
		SaveChanges();

		return OrderCommandsModuleDatabase.ORDER_WAS_REMOVED;
	}

	public Queue<OrderData> GetTodayOrders ()
	{
		return StoreOrdersDataInQueue(GetTodayOrdersEnumerable());
	}

	public Queue<OrderData> GetTodayOrders (string userName)
	{
		return StoreOrdersDataInQueue(GetTodayOrdersEnumerable(order => order.OwnerName == userName));
	}

	public VoteData AddNewReminder (SocketCommandContext messageData, VoteReminderInfo reminderInfo)
	{
		VoteData newVote = new()
		{
			GuildID = messageData.Guild.Id,
			ChannelID = messageData.Message.Channel.Id,
			StartTime = reminderInfo.StartTime,
			StartMessage = reminderInfo.StartMessage,
			DurationInMinutes = reminderInfo.DurationInSeconds,
			FinishMessage = reminderInfo.FinishMessage
		};

		Add(newVote);
		SaveChanges();
		return newVote;
	}

	public Stack<VoteData> GetAllRemindersFromDB ()
	{
		return new Stack<VoteData>(Votes);
	}

	public void DeleteReminder (VoteData reminderDataToDelete)
	{
		Remove(reminderDataToDelete);
		SaveChanges();
	}

	//Logic was changed, need to update separate stuff with separate commands. 17.12.2021. Artem Yurchenko
	// public void UpdateReminder (VoteData reminderToUpdate)
	// {
	// 	VoteData reminder = Votes.SingleOrDefault(reminderData => reminderData.ID == reminderToUpdate.ID);
	//
	// 	if (reminder == null)
	// 	{
	// 		return;
	// 	}
	//
	// 	reminder.UpdateReminderData(reminderToUpdate);
	// 	SaveChanges();
	// }

	private Queue<OrderData> StoreOrdersDataInQueue (IEnumerable<OrderData> records)
	{
		using IEnumerator<OrderData> ordersEnumerator = records.GetEnumerator();
		Queue<OrderData> todayOrders = new();

		while (ordersEnumerator.MoveNext() == true)
		{
			todayOrders.Enqueue(ordersEnumerator.Current);
		}

		return todayOrders;
	}

	private IEnumerable<OrderData> GetTodayOrdersEnumerable (Expression<Func<OrderData, bool>>? additionalCheckExpression = null)
	{
		IQueryable<OrderData> todayOrdersQuery = Orders.Where(order => order.Day == DateTime.Today);

		return additionalCheckExpression == null ? todayOrdersQuery : todayOrdersQuery.Where(additionalCheckExpression);
	}

	private OrderData? FindOrder (OrderInfo orderInfo)
	{
		return Orders.SingleOrDefault(order => (order.OwnerName == orderInfo.OwnerName) && (order.ID == orderInfo.ID));
	}
}