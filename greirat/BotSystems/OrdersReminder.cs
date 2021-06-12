using System;
using System.Threading.Tasks;

namespace greirat
{
    public class OrdersReminder
    {
        private FoodRemindData ReminderData { get; set; }
        private TimeSpan RemindTime { get; set; }

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
            RemindTime = TimeSpan.Parse(ReminderData.TimeToRemind);
            
            while (true)
            {
                TimeSpan timeToWait = CalculateTimeToRemind();
                await Task.Delay(timeToWait);
                await Program.Bot.SendMessage(ReminderData.GuildID, ReminderData.ChannelID, ReminderData.RemindMessage);
            }
        }

        private TimeSpan CalculateTimeToRemind ()
        {
            TimeSpan currentTime = DateTime.Now.TimeOfDay;
            TimeSpan timeToWait = currentTime < RemindTime ? RemindTime.Subtract(currentTime) : DateTime.Today.Subtract(currentTime).TimeOfDay + RemindTime;
            
            return timeToWait;
        }
    }
}