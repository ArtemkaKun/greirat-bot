﻿using System.Threading.Tasks;

namespace greirat
{
    internal class Program
    {
        public static DiscordBot Bot { get; private set; } = new();
        public static DB DBManager { get; private set; } = new();
        
        private static void Main ()
        {
            MainAsync().GetAwaiter().GetResult();
        }

        private static async Task MainAsync ()
        {
            await Bot.Initialize();
            await Task.Delay(-1);
        }
    }
}