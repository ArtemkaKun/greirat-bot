namespace greirat
{
    public class SimpleReminderInfo
    {
        public string RemindTime { get; private set; }
        public string RemindMessage { get; private set; }

        public SimpleReminderInfo (string remindTime, string remindMessage)
        {
            RemindTime = remindTime;
            RemindMessage = remindMessage;
        }
    }
}