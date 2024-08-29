namespace GoDota2_Bot
{
    internal class Program
    {
        internal static void Main()
        {
            PowerUsageMonitor.RunPowerMonitors().Wait();

            ConfigureZones configureZones = new ConfigureZones();
            configureZones.Configure();

            BotLogic.MainBot();
            while (true)
            {
                MainLogic mainLogic = new MainLogic();
                mainLogic.AutoBet();               
            }
        }
    }
}
