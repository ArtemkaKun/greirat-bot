namespace greirat
{
    public class BotConfigData
    {
        public ulong GuildIDForTests { get; set; }
        public ulong ChannelIDForTests { get; set; }

        public BotConfigData (ulong guildIdForTests, ulong channelIdForTests)
        {
            GuildIDForTests = guildIdForTests;
            ChannelIDForTests = channelIdForTests;
        }
    }
}