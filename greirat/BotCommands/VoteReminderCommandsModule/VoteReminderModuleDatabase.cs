namespace greirat
{
    public static class VoteReminderModuleDatabase
    {
        public const string VOTE_REMINDER_COMMANDS_GROUP_NAME = "voteReminder";
        
        public const string CANNOT_SET_REMINDER_MESSAGE = "Cannot set reminder for this channel";
        public const string NO_REMINDER_IN_CHANNEL_MESSAGE = "No reminders in channet yet";
        public const string REMINDER_WAS_REMOVED_MESSAGE = "Reminder was successfully removed";
        public const string REMINDER_WAS_UPDATED_MESSAGE = "Reminder was successfully updated";
        
        public const string SET_REMINDER_COMMAND_DESCRIPTION = "Sets food vote reminder";
        public const string SHOW_REMINDER_COMMAND_DESCRIPTION = "Show channel's reminder data";
        public const string DELETE_REMINDER_COMMAND_DESCRIPTION = "Delete channel's reminder";
        public const string UPDATE_REMINDER_COMMAND_DESCRIPTION = "Update channel's reminder";
    }
}