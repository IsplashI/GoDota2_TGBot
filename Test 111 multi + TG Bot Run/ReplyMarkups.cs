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
        new[] { new KeyboardButton("/change_balance") },
        new[] { new KeyboardButton("/change_current_balance") },
        new[] { new KeyboardButton("/change_bet_limits") },
        new[] { new KeyboardButton("/change_min_profit") },
        new[] { new KeyboardButton("/advanced_buttons") },

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
        public static IReplyMarkup GetAdvancedButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { new KeyboardButton("/start") },
        new[] { new KeyboardButton("/pause_betting") },       
        new[] { new KeyboardButton("/capture_screen") },
        new[] { new KeyboardButton("/get_system_info") },
        new[] { new KeyboardButton("/get_software_info") },
        new[] { new KeyboardButton("/get_power_usage") },
        new[] { new KeyboardButton("/shutdown_ask") },})
            {
                ResizeKeyboard = true
            };
        }
        public static IReplyMarkup GetAcceptionButtons()
        {
            return new ReplyKeyboardMarkup(new[]
            {
        new[] { new KeyboardButton("/start") },
        new[] { new KeyboardButton("/shutdown_pc") },})
            {
                ResizeKeyboard = true
            };
        }

    }
}
