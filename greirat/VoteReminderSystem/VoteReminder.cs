using System;
using System.Threading;
using System.Threading.Tasks;
using DBSystem;
using greirat;

namespace VoteReminderSystem
{
	public class VoteReminder
	{
		public VoteData ReminderData { get; private set; }
		
		private Task ReminderTimerTask { get; set; }
		private CancellationTokenSource ReminderCancellationProvider { get; set; }
		private CancellationToken ReminderCancellationToken { get; set; }

		public VoteReminder (VoteData reminderData)
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

			ReminderTimerTask = new Task(RemindAboutVote);
			ReminderTimerTask.Start();
		}

		public void CancelActualReminderThread ()
		{
			ReminderCancellationProvider.Cancel();
			ReminderTimerTask.Dispose();
		}

		//Logic was changed, need to update separate stuff with separate commands. 17.12.2021. Artem Yurchenko
		// public void UpdateReminderData (VoteReminderInfo newData)
		// {
		// 	ReminderData.UpdateReminderData(newData);
		// }

		private void SetUpCancellationMembers ()
		{
			ReminderCancellationProvider = new CancellationTokenSource();
			ReminderCancellationToken = ReminderCancellationProvider.Token;
		}

		private async void RemindAboutVote ()
		{
			TimeSpan voteDuration = new(0, ReminderData.DurationInMinutes, 0);

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

				await Program.BotClient.SendMessage(ReminderData.GuildID, ReminderData.ChannelID, string.IsNullOrEmpty(ReminderData.StartMessage) == false ? ReminderData.StartMessage : "Let's order some food");

				try
				{
					await Task.Delay(voteDuration, ReminderCancellationToken);
				}
				catch (Exception)
				{
					return;
				}

				await Program.BotClient.SendMessage(ReminderData.GuildID, ReminderData.ChannelID, string.IsNullOrEmpty(ReminderData.FinishMessage) == false ? ReminderData.FinishMessage : "Food voting was finished");
			}
		}

		private TimeSpan CalculateTimeToRemind ()
		{
			TimeSpan currentTime = DateTime.Now.TimeOfDay;
			TimeSpan timeToWait = currentTime < ReminderData.StartTime ? ReminderData.StartTime.Subtract(currentTime) : DateTime.Today.Subtract(currentTime).TimeOfDay + ReminderData.StartTime;

			return timeToWait;
		}

		private bool CheckIfTodayIsWeekend ()
		{
			DayOfWeek todayDay = DateTime.Today.DayOfWeek;
			return todayDay is DayOfWeek.Saturday or DayOfWeek.Sunday;
		}
	}
}