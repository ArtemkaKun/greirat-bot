using System;
using System.Threading.Tasks;

namespace greirat
{
    public class OrdersReminder
    {
        private FoodRemindData ReminderData { get; set; }

        public OrdersReminder (FoodRemindData reminderId)
        {
            ReminderData = reminderId;
        }

        public void TryStartReminderThread ()
        {
            if (CheckIfCanStartReminderThread() == true)
            {
                RemindAboutFood();
            }
        }

        private bool CheckIfCanStartReminderThread ()
        {
            string timeForRemind = ReminderData.TimeToRemind;
            return (string.IsNullOrEmpty(timeForRemind) == false) && (TimeSpan.TryParse(timeForRemind, out _) == true);
        }

        private async Task RemindAboutFood ()
        {
            while (true)
            {
                TimeSpan timeToWait = CalculateTimeToRemind();
                await Task.Delay(timeToWait);
                await Program.Bot.SendMessage(ReminderData.GuildID, ReminderData.ChannelID, "@here TEST");
            }
        }

        private TimeSpan CalculateTimeToRemind ()
        {
            TimeSpan remindTime = TimeSpan.Parse(ReminderData.TimeToRemind);
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan timeToWait = currentTime < remindTime ? remindTime.Subtract(currentTime) : DateTime.Today.Subtract(currentTime).TimeOfDay + remindTime;
            
            return timeToWait;
        }
    }
}