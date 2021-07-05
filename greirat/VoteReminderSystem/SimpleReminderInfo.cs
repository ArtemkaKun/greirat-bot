namespace VoteReminderSystem
{
    public class SimpleReminderInfo
    {
        public string RemindTime { get; }
        public string RemindMessage { get; }

        public SimpleReminderInfo (string remindTime, string remindMessage)
        {
            RemindTime = remindTime;
            RemindMessage = remindMessage;
        }
    }
}