using System;
using System.Threading.Tasks;

namespace greirat
{
    public class OrdersReminder
    {
        public FoodRemindData ReminderData { get; private set; }
        
        private TimeSpan RemindTime { get; set; }

        public OrdersReminder (FoodRemindData reminderData)
        {
            ReminderData = reminderData;
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
            if (CheckIfTodayIsWeekend() == true)
            {
                return new TimeSpan(24, 0, 0);
            }
            
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