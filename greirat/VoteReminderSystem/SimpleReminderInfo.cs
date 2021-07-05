namespace VoteReminderSystem
{
	public class SimpleReminderInfo
	{
		public string Time { get; }
		public string Message { get; }

		public SimpleReminderInfo (string time, string message)
		{
			Time = time;
			Message = message;
		}
	}
}