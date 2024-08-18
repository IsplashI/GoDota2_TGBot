using Telegram.Bot;
using Telegram.Bot.Types;

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
