using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSystem
{
	[Table("VotingReminders")]
	public class VoteData
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int ID { get; init; }
		public ulong GuildID { get; init; }
		public ulong ChannelID { get; init; }
		public TimeSpan StartTime { get; init; }
		[MaxLength(500)]
		public string StartMessage { get; init; } = null!;
		public int DurationInMinutes { get; init; }
		[MaxLength(500)]
		public string FinishMessage { get; init; } = null!;
	}
}