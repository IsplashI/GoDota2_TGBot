using Telegram.Bot.Types.ReplyMarkups;

namespace GoDota2_Bot
{
    internal class ReplyMarkups
    {
        public static IReplyMarkup GetDefaultButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { new KeyboardButton("/start") },
        new[] { new KeyboardButton("/show_info") },
        new[] { new KeyboardButton("/capture_screen") },
        new[] { new KeyboardButton("/change_balance") },
        new[] { new KeyboardButton("/change_current_balance") },
        new[] { new KeyboardButton("/change_bet_limits") },
        new[] { new KeyboardButton("/change_min_profit") },
                    })
            {
                ResizeKeyboard = true
            };
        }

        public static IReplyMarkup GetLimitButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { new KeyboardButton("/start") },
        new[] { new KeyboardButton("/change_green_limit") },
        new[] { new KeyboardButton("/change_red_limit") },
        new[] { new KeyboardButton("/change_black_limit") }                           })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
        
        public static IReplyMarkup GetMinProfitButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { new KeyboardButton("/start") },
        new[] { new KeyboardButton("/change_min_profit_all") },
        new[] { new KeyboardButton("/change_min_profit_green") },
        new[] { new KeyboardButton("/change_min_profit_red") },
        new[] { new KeyboardButton("/change_min_profit_black") }                           })
            {
                ResizeKeyboard = true,
                OneTimeKeyboard = true
            };
        }
    }
}
