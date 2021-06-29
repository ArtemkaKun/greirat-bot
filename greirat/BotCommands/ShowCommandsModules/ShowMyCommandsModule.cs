﻿using System.Collections.Generic;
using System.Threading.Tasks;
using Discord.Commands;
using greirat;

namespace BotCommands
{
    [Group("shmy")]
    public class ShowMyCommandsModule : BaseShowCommandsModule
    {
        [Command(COMMON_SHOW_COMMAND_NAME)]
        [Summary("Shows your today's orders")]
        public Task ShowUserTodayOrders ()
        {
            return ShowOrdersData(COMMON_SHOW_COMMAND_NAME, GetAllUserTodayOrders());
        }
        
        [Command(SORT_SHOW_COMMAND_NAME)]
        [Summary("Shows your today's orders (sorted)")]
        public Task ShowUserTodayOrdersSorted ()
        {
            return ShowOrdersData(SORT_SHOW_COMMAND_NAME, GetAllUserTodayOrders());
        }
        
        [Command(SUM_SHOW_COMMAND_NAME)]
        [Summary("Shows your today's orders (summary)")]
        public Task ShowUserTodayOrdersSummary ()
        {
            return ShowOrdersData(SUM_SHOW_COMMAND_NAME, GetAllUserTodayOrders());
        }

        private Queue<OrderData> GetAllUserTodayOrders ()
        {
            return Program.DBManager.GetTodayOrders(Context.Message.Author.Username);
        }
    }
}