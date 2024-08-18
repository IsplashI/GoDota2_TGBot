using Telegram.Bot;
using Telegram.Bot.Types;
using Test_111_multi___TG_Bot_Run;

namespace GoDota2_Bot
{
    public class Host
    {
        
        public Action<ITelegramBotClient, Update>? OnMessage;        
        private TelegramBotClient _bot;        
        public Host(string token) 
        {
            BotConfiguration botConfiguration = new BotConfiguration();
            _bot = new TelegramBotClient(token);
        }
        public void Start()
        {
            _bot.StartReceiving(UpdateHandler, ErrorHandler);
            OnlineMessage().Wait();
            Console.WriteLine("online");
        }

        public async Task OnlineMessage()
        {
            if (BotConfiguration.chatId != 0)
            {
                await _bot.SendTextMessageAsync(BotConfiguration.chatId, "online");
            }                    
        }
        

        public async Task SendInfo()
        {
            int balance = MainLogic.balance;
            int round = MainLogic.round;
            string currentColor = MainLogic.currentColor;

            int notGreen = MainLogic.notGreen;
            int notRed = MainLogic.notRed;
            int notBlack = MainLogic.notBlack;

            int greenLimit = MainLogic.greenLimit;
            int redLimit = MainLogic.redLimit;
            int blackLimit = MainLogic.blackLimit;

            double greenCount = MainLogic.greenCount;
            double redCount = MainLogic.redCount;
            double blackCount = MainLogic.blackCount;

            double greenProbab = MainLogic.greenProbab;
            double redProbab = MainLogic.redProbab;
            double blackProbab = MainLogic.blackProbab;

            bool bettingGreen = MainLogic.bettingGreen;
            bool bettingRed = MainLogic.bettingGreen;
            bool bettingBlack = MainLogic.bettingGreen;

            
            
          
            if (BotLogic.show == true || BotLogic.show1 == true)
            {
                string message1 = $"Round: {round}\nBalance: {balance}\nCurrent color: {currentColor}\n\n" +
                    $"Red {notRed}/{redLimit}\nBlack {notBlack}/{blackLimit}\nGreen {notGreen}/{greenLimit}\n\n" +
                    $"Red count:   {redProbab}   {redCount}\nBlack count: {blackProbab}   {blackCount}\nGreen count {greenProbab}   {greenCount}";

                await _bot.SendTextMessageAsync(BotConfiguration.chatId, message1);            
            }
            
            
            
        }

        private async Task ErrorHandler(ITelegramBotClient client, Exception exception, CancellationToken token)
        {
            Console.WriteLine("Error:" + exception.Message);
            await Task.CompletedTask;
        }

        private async Task UpdateHandler(ITelegramBotClient client, Update update, CancellationToken token)
        {
            Console.WriteLine($"New message: {update.Message?.Text ?? "not text"}");
            OnMessage?.Invoke(client, update);
            await Task.CompletedTask;

            UserInput userInput = new UserInput();
            await userInput.UserInputHandler(client, update);            
        }       
    }
}
