namespace GoDota2_Bot
{
    internal class Program
    {
        internal static void Main()
        {                       
            BotLogic.MainBot();
            while (true)
            {
                MainLogic.AutoBet();               
            }
        }
    }
}
