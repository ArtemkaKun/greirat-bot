using System;
using System.Threading;
using System.Threading.Tasks;
using DBSystem;
using greirat;

namespace VoteReminderSystem
{
	public class VoteReminder
	{
		public VoteRemindData ReminderData { get; private set; }

		private TimeSpan RemindTime { get; set; }
		private Task ReminderTimerTask { get; set; }
		private CancellationTokenSource ReminderCancellationProvider { get; set; }
		private CancellationToken ReminderCancellationToken { get; set; }

		public VoteReminder (VoteRemindData reminderData)
		{
			ReminderData = reminderData;
			SetUpCancellationMembers();
		}

		public void TryStartReminderThread ()
		{
			if (ReminderTimerTask != null)
			{
				CancelActualReminderThread();
				SetUpCancellationMembers();
			}

			if (CheckIfCanStartReminderThread() == true)
			{
				ReminderTimerTask = new Task(RemindAboutVote);
				ReminderTimerTask.Start();
			}
		}

		public void CancelActualReminderThread ()
		{
			ReminderCancellationProvider.Cancel();
			ReminderTimerTask.Dispose();
		}

		public void UpdateReminderData (SimpleReminderInfo newData)
		{
			ReminderData.UpdateReminderData(newData);
		}

		private void SetUpCancellationMembers ()
		{
			ReminderCancellationProvider = new CancellationTokenSource();
			ReminderCancellationToken = ReminderCancellationProvider.Token;
		}

		private bool CheckIfCanStartReminderThread ()
		{
			string timeForRemind = ReminderData.TimeToRemind;
			return (string.IsNullOrEmpty(timeForRemind) == false) && (TimeSpan.TryParse(timeForRemind, out _) == true);
		}

		private async void RemindAboutVote ()
		{
			RemindTime = TimeSpan.Parse(ReminderData.TimeToRemind);
			TimeSpan voteDuration = new(0, ReminderData.VoteDurationInMinutes, 0);

			while (true)
			{
				TimeSpan timeToWait = CalculateTimeToRemind();

				try
				{
					await Task.Delay(timeToWait, ReminderCancellationToken);
				}
				catch (Exception)
				{
					return;
				}

				if (CheckIfTodayIsWeekend() == true)
				{
					continue;
				}

				await Program.BotClient.SendMessage(ReminderData.GuildID, ReminderData.ChannelID, string.IsNullOrEmpty(ReminderData.RemindMessage) == false ? ReminderData.RemindMessage : "Let's order some food");

				try
				{
					await Task.Delay(voteDuration, ReminderCancellationToken);
				}
				catch (Exception)
				{
					return;
				}

				await Program.BotClient.SendMessage(ReminderData.GuildID, ReminderData.ChannelID, string.IsNullOrEmpty(ReminderData.VoteFinishMessage) == false ? ReminderData.VoteFinishMessage : "Food voting was finished");
			}
		}

		private TimeSpan CalculateTimeToRemind ()
		{
			TimeSpan currentTime = DateTime.Now.TimeOfDay;
			TimeSpan timeToWait = currentTime < RemindTime ? RemindTime.Subtract(currentTime) : DateTime.Today.Subtract(currentTime).TimeOfDay + RemindTime;

			return timeToWait;
		}

		private bool CheckIfTodayIsWeekend ()
		{
			DayOfWeek todayDay = DateTime.Today.DayOfWeek;
			return todayDay is DayOfWeek.Saturday or DayOfWeek.Sunday;
		}
	}
}