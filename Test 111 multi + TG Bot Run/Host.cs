using Telegram.Bot;
using Telegram.Bot.Types;

namespace GoDota2_Bot
{
    public class Host
    {

        public Action<ITelegramBotClient, Update>? OnMessage;
        private TelegramBotClient _bot;
        public Host()
        {
            var botConfiguration = BotConfiguration.Configuration;
            try
            {
                _bot = new TelegramBotClient(botConfiguration.botToken);
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
            
        }
        public void Start()
        {
            try
            {
                _bot.StartReceiving(UpdateHandler, ErrorHandler);
                OnlineMessage().Wait();
                Console.WriteLine("online");
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }            
        }

        public async Task OnlineMessage()
        {
            BotConfiguration.Configuration = BotConfiguration.Read(BotConfiguration.ConfigFilePath);

            foreach (var chatId in BotConfiguration.Configuration.chatIds)
            {
                await _bot.SendTextMessageAsync(chatId, "online");
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
