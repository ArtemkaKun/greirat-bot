using System.IO;
using System.Text.Json;

namespace greirat
{
	public class BotConfig
	{
		private const string PATH_TO_CONFIG_FILE = "config.txt";

		public BotConfigData CurrentConfigData { get; private set; }

		public BotConfig ()
		{
			InitializeConfig();
		}

		private void InitializeConfig ()
		{
			if (File.Exists(PATH_TO_CONFIG_FILE) == true)
			{
				CurrentConfigData = JsonSerializer.Deserialize<BotConfigData>(File.ReadAllText(PATH_TO_CONFIG_FILE));
			}
			else
			{
				CurrentConfigData = new BotConfigData(default, default);
				File.WriteAllText(PATH_TO_CONFIG_FILE, JsonSerializer.Serialize(CurrentConfigData));
			}
		}
	}
}