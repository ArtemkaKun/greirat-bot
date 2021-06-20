using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

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

        public VoteRemindData (ulong guildID, ulong channelID, string timeToRemind, string remindMessage)
        {
            GuildID = guildID;
            ChannelID = channelID;
            TimeToRemind = timeToRemind;
            RemindMessage = remindMessage;
        }
    }
}