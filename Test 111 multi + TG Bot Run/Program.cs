using GoDota2_Bot;

namespace Test_111_multi___TG_Bot_Run
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
