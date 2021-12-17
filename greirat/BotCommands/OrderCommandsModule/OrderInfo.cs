namespace BotCommands;

public record OrderInfo
{
	public int ID { get; init; }
	public string? Text { get; init; }
	public string OwnerName { get; init; } = null!;
}