using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DBSystem
{
	[Table("Orders")]
	public class OrderData
	{
		[DatabaseGenerated(DatabaseGeneratedOption.Identity)]
		[Key]
		public int ID { get; init; }
		public DateTime Day { get; init; }
		[MaxLength(50)]
		public string OwnerName { get; init; } = null!;
		[MaxLength(500)]
		public string Text { get; init; } = null!;
	}
}