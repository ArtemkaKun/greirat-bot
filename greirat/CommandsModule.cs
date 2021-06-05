﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using greirat.Helpers;

namespace greirat
{
    public class CommandsModule : ModuleBase<SocketCommandContext>
    {
        private const string ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE = "Order was successfully ";
        private const string ORDER_WAS_SAVED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "proceeded";
        private const string ORDER_WAS_UPDATED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "updated";
        private const string ORDER_WAS_REMOVED = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "removed";
        private const string ORDER_UPDATE_FAILED = "Failed to update order";
        private const string ORDER_DELETE_FAILED = "Failed to remove order";
        private const string NOTHING_TO_SHOW_MESSAGE = "Nothing to show";

        private OrderDataAsciiTableConverter AsciiTableConverter { get; set; } = new ();

        [Command("makeOrder")]
        [Summary("Creates new order")]
        public Task CreateNewOrder ([Remainder] string orderText)
        {
            DB.Instance.AddNewOrder(Context.Message.Author.Username, orderText);
            
            return ReplyAsync(ORDER_WAS_SAVED_MESSAGE);
        }
        
        [Command("showTodayOrders")]
        [Summary("Shows today's orders")]
        public Task ShowTodayOrders ()
        {
            Queue<OrderData> todayOrders = DB.Instance.GetTodayOrders();
            
            return ReplyAsync(todayOrders.Count == 0 ? NOTHING_TO_SHOW_MESSAGE : AsciiTableConverter.FormOrdersShowData(todayOrders).ToString());
        }

        [Command("showMyTodayOrders")]
        [Summary("Shows users today's orders")]
        public Task ShowUserTodayOrders ()
        {
            Queue<OrderData> todayOrders = DB.Instance.GetTodayOrders(Context.Message.Author.Username);
            
            return ReplyAsync(todayOrders.Count == 0 ? NOTHING_TO_SHOW_MESSAGE : AsciiTableConverter.FormOrdersShowData(todayOrders).ToString());
        }

        [Command("updateOrder")]
        [Summary("Updates order with provided ID")]
        public Task UpdateUserOrder (int idOfOrder, [Remainder] string newOrderMessage)
        {
            bool updateOperationResult = DB.Instance.TryUpdateOrderData(Context.Message.Author.Username, idOfOrder, newOrderMessage);
            
            return ReplyAsync(updateOperationResult == true ? ORDER_WAS_UPDATED_MESSAGE : ORDER_UPDATE_FAILED);
        }

        [Command("deleteOrder")]
        [Summary("Deletes order with provided ID")]
        public Task DeleteOrder (int idOfOrder)
        {
            bool deleteOperationResult = DB.Instance.TryDeleteOrderData(Context.Message.Author.Username, idOfOrder);
            
            return ReplyAsync(deleteOperationResult == true ? ORDER_WAS_REMOVED : ORDER_DELETE_FAILED);
        }
    }
}