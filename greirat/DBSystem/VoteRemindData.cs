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
		public string TimeToRemind { get; set; }
		public string RemindMessage { get; set; }
		public int VoteDurationInMinutes { get; set; }

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
			TimeToRemind = reminderInfo.RemindTime;
			RemindMessage = reminderInfo.RemindMessage;
			VoteDurationInMinutes = 60;
		}
	}
}