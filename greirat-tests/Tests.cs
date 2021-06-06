using System.Threading;
using System.Threading.Tasks;
using greirat;
using NUnit.Framework;

namespace greirat_tests
{
    public class Tests
    {
        private DiscordBot Client { get; set; } = new();
        
        [OneTimeSetUp]
        public async Task PrepareTestEnvironment ()
        {
            SemaphoreSlim eventWaiterSemaphore = new(0, 1);

            Client.BotClient.Ready += async () =>
            {
                await Client.SendMessageToChat();
                eventWaiterSemaphore.Release();
            };
            
            await Client.Initialize();
            await eventWaiterSemaphore.WaitAsync();
        }
        
        [Test]
        public void CreateNewOrderAndCheckIfExistsShouldBeTrue ()
        {
            
        }
    }
}