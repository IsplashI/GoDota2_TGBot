using Newtonsoft.Json;

namespace GoDota2_Bot
{
    public class BotConfiguration
    {
        public string botToken { get; set; }
        public static long chatId { get; set; }

        public SortedSet<long> chatIds { get; set; }

        public static BotConfiguration Configuration 
        {   
            get 
            { 
                if (botConfiguration == null) 
                { 
                    botConfiguration = new BotConfiguration();
                    botConfiguration.chatIds = new SortedSet<long>();
                    botConfiguration.botToken = GetToken();
                }
                return botConfiguration;
            }
            set 
            { 
                botConfiguration = value;
            }
        }


        private static BotConfiguration botConfiguration { get; set; }

        public const string ConfigFilePath = "BotConfiguration.json";
        
        public static void JsonToFile(object Object, string path)
        {
            JsonSerializerSettings options = new JsonSerializerSettings();             
            options.Formatting = Formatting.Indented;                                   
            string json = JsonConvert.SerializeObject(Object, typeof(Object), options); 
            File.WriteAllText(path, json);                                     
        }
        
        public static BotConfiguration Read(string path)
        {
            string json = File.ReadAllText(path);
            var Object = JsonConvert.DeserializeObject<BotConfiguration>(json);
            return Object;                         
        }
               
        public static string GetToken()
        {
            if (!File.Exists(ConfigFilePath))
            {
                var token = InputToken();
                SaveToken(token);
                return token;
            }

            return LoadToken();
        }

        private static string LoadToken()
        {
            Configuration = Read(ConfigFilePath);

            return Configuration.botToken;
        }

        private static string InputToken()
        {
            string token = "YourBotToken";
            while (token.Length != 46)
            {
                Console.WriteLine("Please enter a 46 character long bot token:");
                token = Console.ReadLine() ?? string.Empty;
                Console.Clear();
            }
            
            return token;
        }

        private static void SaveToken(string token)
        {
            BotConfiguration.Configuration.botToken = token;
            JsonToFile(BotConfiguration.Configuration, ConfigFilePath);
        }        
    }    
}
    
