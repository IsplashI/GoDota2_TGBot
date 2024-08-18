using Telegram.Bot.Types;
using Telegram.Bot;
using GoDota2_Bot;

namespace GoDota2_Bot
{
    internal class UserInput
    {
        public async Task UserInputHandler(ITelegramBotClient client, Update update)
        {
            await BalanceInputHandler(client, update);
            await LimitInputHandler(client, update);
            await MinProfitInputHandler(client, update);
            await Task.CompletedTask;
        }
        private async Task BalanceInputHandler(ITelegramBotClient client, Update update)
        {
            if (BotLogic.waitingBalance == true)
            {
                string messageText = update.Message?.Text ?? "not text";
                if (int.TryParse(messageText, out int balance) && balance > 0)
                {
                    MainLogic.balance = balance;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nBalance:{balance}");
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!");
                }
                BotLogic.waitingBalance = false;
            }
        }
        private async Task LimitInputHandler(ITelegramBotClient client, Update update)
        {
            if (BotLogic.waitingGreenLimit == true)
            {
                string messageText = update.Message?.Text ?? "not text";

                if (int.TryParse(messageText, out int greenLimit) && greenLimit >= 0)
                {
                    MainLogic.greenLimit = greenLimit;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nGreen limit:{greenLimit}", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                BotLogic.waitingGreenLimit = false;
            }

            if (BotLogic.waitingRedLimit == true)
            {
                string messageText = update.Message?.Text ?? "not text";

                if (int.TryParse(messageText, out int redLimit) && redLimit >= 0)
                {
                    MainLogic.redLimit = redLimit;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nGreen limit:{redLimit}", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                BotLogic.waitingRedLimit = false;
            }

            if (BotLogic.waitingBlackLimit == true)
            {
                string messageText = update.Message?.Text ?? "not text";

                if (int.TryParse(messageText, out int blackLimit) && blackLimit >= 0)
                {
                    MainLogic.blackLimit = blackLimit;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nGreen limit:{blackLimit}", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                BotLogic.waitingBlackLimit = false;
            }
        }
        private async Task MinProfitInputHandler(ITelegramBotClient client, Update update)
        {
            if (BotLogic.waitingAllMinProfit == true)
            {
                string messageText = update.Message?.Text ?? "not text";

                if (int.TryParse(messageText, out int minProfitAll) && minProfitAll > 0)
                {
                    MainLogic.minProfitGreen = minProfitAll;
                    MainLogic.minProfitRed = minProfitAll;
                    MainLogic.minProfitBlack = minProfitAll;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nAll minimal profit: {minProfitAll}", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                BotLogic.waitingAllMinProfit = false;
            }
            if (BotLogic.waitingGreenMinProfit == true)
            {
                string messageText = update.Message?.Text ?? "not text";

                if (int.TryParse(messageText, out int minProfitGreen) && minProfitGreen > 0)
                {
                    MainLogic.minProfitGreen = minProfitGreen;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nGreen minimal profit: {minProfitGreen}", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                BotLogic.waitingGreenMinProfit = false;
            }

            if (BotLogic.waitingRedMinProfit == true)
            {
                string messageText = update.Message?.Text ?? "not text";

                if (int.TryParse(messageText, out int minProfitRed) && minProfitRed > 0)
                {
                    MainLogic.minProfitRed = minProfitRed;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nRed minimal profit: {minProfitRed}", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                BotLogic.waitingRedLimit = false;
            }

            if (BotLogic.waitingBlackMinProfit == true)
            {
                string messageText = update.Message?.Text ?? "not text";

                if (int.TryParse(messageText, out int minProfitBlack) && minProfitBlack > 0)
                {
                    MainLogic.minProfitBlack = minProfitBlack;
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Succes!\nBlack minimal profit: {minProfitBlack}", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                else
                {
                    await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Error!", replyMarkup: ReplyMarkups.GetDefaultButtons());
                }
                BotLogic.waitingBlackMinProfit = false;
            }
        }
    }
}
