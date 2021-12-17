using System;

namespace VoteReminderSystem
{
	public record VoteReminderInfo(TimeSpan StartTime, string StartMessage, int DurationInSeconds, string FinishMessage);
}