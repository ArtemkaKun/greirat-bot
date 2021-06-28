﻿using System.Runtime.InteropServices;
using System.Threading.Tasks;
using Discord.Commands;

namespace greirat
{
    [Group(VOTE_REMINDER_COMMANDS_GROUP_NAME)]
    public class VoteReminderCommands : ModuleBase<SocketCommandContext>
    {
        private const string VOTE_REMINDER_COMMANDS_GROUP_NAME = "voteReminder";
        private const string REMINDER_WAS_SET_MESSAGE = "Reminder was set on {0} everyday (except weekends)";
        private const string CANNOT_SET_REMINDER_MESSAGE = "Cannot set reminder for this channel";
        private const string NO_REMINDER_IN_CHANNEL_MESSAGE = "No reminders in channet yet";
        private const string REMINDER_WAS_REMOVED_MESSAGE = "Reminder was successfully removed";
        private const string REMINDER_WAS_UPDATED_MESSAGE = "Reminder was successfully updated";

        [Command(CommandsDatabase.SET_COMMAND_NAME)]
        [Summary("Sets food vote reminder")]
        public Task SetEverydayVoteReminder (string remindTime, [Optional] int voteDurationInMinutes, [Remainder] string remindMessage)
        {
            bool isOperationSucceed = Program.VoteRemindersController.TryStartNewVoteReminder(Context, remindTime, remindMessage, voteDurationInMinutes);

            return ReplyAsync(isOperationSucceed == true ? string.Format(REMINDER_WAS_SET_MESSAGE, remindTime) : CANNOT_SET_REMINDER_MESSAGE);
        }

        [Command(CommandsDatabase.SHOW_COMMAND_NAME)]
        [Summary("Show channel's reminder data")]
        public Task ShowChannelVoteReminderData ()
        {
            string infoAboutReminder = Program.VoteRemindersController.GetVoteReminderInfo(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(string.IsNullOrEmpty(infoAboutReminder) == true ? NO_REMINDER_IN_CHANNEL_MESSAGE : infoAboutReminder);
        }

        [Command(CommandsDatabase.DELETE_COMMAND_NAME)]
        [Summary("Delete channel's reminder")]
        public Task DeleteChannelVoteReminder ()
        {
            bool isOperationSucceed = Program.VoteRemindersController.TryDeleteChannelVoteReminder(Context.Guild.Id, Context.Message.Channel.Id);

            return ReplyAsync(isOperationSucceed == true ? REMINDER_WAS_REMOVED_MESSAGE : NO_REMINDER_IN_CHANNEL_MESSAGE);
        }

        [Command(CommandsDatabase.UPDATE_COMMAND_NAME)]
        [Summary("Update channel's reminder")]
        public Task UpdateChannelVoteReminder (string remindTime, [Remainder] string remindMessage)
        {
            bool isOperationSucceed = Program.VoteRemindersController.TryUpdateChannelVoteReminder(Context, remindTime, remindMessage);

            return ReplyAsync(isOperationSucceed == true ? REMINDER_WAS_UPDATED_MESSAGE : NO_REMINDER_IN_CHANNEL_MESSAGE);
        }
    }
}