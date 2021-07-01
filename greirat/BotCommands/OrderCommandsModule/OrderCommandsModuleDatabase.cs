namespace BotCommands
{
    public static class OrderCommandsModuleDatabase
    {
        public const string ORDER_COMMANDS_GROUP_NAME = "order";
        public const string CREATE_NEW_ORDER_COMMAND_DESCRIPTION = "Creates a new order";
        public const string UPDATE_USER_ORDER_COMMAND_DESCRIPTION = "Updates order with provided text";
        public const string DELETE_ORDER_COMMAND_DESCRIPTION = "Deletes order";
        public const string ORDER_WAS_SAVED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "proceeded";
        public const string ORDER_WAS_UPDATED_MESSAGE = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "updated";
        public const string ORDER_WAS_REMOVED = ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE + "removed";
        public const string ORDER_UPDATE_FAILED = "Failed to update order";
        public const string ORDER_DELETE_FAILED = "Failed to remove order";
        
        private const string ORDER_WAS_SUCCESSFULLY_TEMPLATE_MESSAGE = "Order was successfully ";
    }
}