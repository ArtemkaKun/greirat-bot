using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Discord.Commands;
using VoteReminderSystem;

namespace greirat
{
	[Table("FoodReminders")]
	public class VoteRemindData
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int ReminderID { get; private set; }
		public ulong GuildID { get; private set; }
		public ulong ChannelID { get; private set; }
		public string TimeToRemind { get; private set; }
		public string RemindMessage { get; private set; }
		public int VoteDurationInMinutes { get; private set; }

		public VoteRemindData (ulong guildID, ulong channelID, string timeToRemind, string remindMessage, int voteDurationInMinutes)
		{
			GuildID = guildID;
			ChannelID = channelID;
			TimeToRemind = timeToRemind;
			RemindMessage = remindMessage;
			VoteDurationInMinutes = voteDurationInMinutes;
		}

		public VoteRemindData (SocketCommandContext messageData, SimpleReminderInfo reminderInfo)
		{
			GuildID = messageData.Guild.Id;
			ChannelID = messageData.Message.Channel.Id;
			TimeToRemind = reminderInfo.Time;
			RemindMessage = reminderInfo.Message;
			VoteDurationInMinutes = 60;
		}

		public void UpdateReminderData (SimpleReminderInfo newData)
		{
			TimeToRemind = newData.Time;
			RemindMessage = newData.Message;
		}
		
		public void UpdateReminderData (VoteRemindData newData)
		{
			TimeToRemind = newData.TimeToRemind;
			RemindMessage = newData.RemindMessage;
		}
	}
}