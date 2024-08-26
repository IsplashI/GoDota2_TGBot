using Newtonsoft.Json;
using System.Drawing;
using System.Runtime.InteropServices;

namespace GoDota2_Bot
{
    internal class ConfigureZones
    {
        [DllImport("user32.dll")]
        static extern bool GetCursorPos(out Point lpPoint);

        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        static extern short GetAsyncKeyState(int vKey);

        static int trash;

        const int VK_Q = 0x51;
        const int VK_ENTER = 0x0D;

        public const string ConfigFilePath = "CoordinatesAndColorConfiguration.json";

        public class Coordinates
        {
            public int x { get; set; }
            public int y { get; set; }

            public int startColorR { get; set; }
            public int startColorG { get; set; }
            public int startColorB { get; set; }

            public int x1 { get; set; }
            public int y1 { get; set; }

            public int cl1 { get; set; }
            public int cl2 { get; set; }

            public int X1 { get; set; }
            public int Y1 { get; set; }
            public int X10 { get; set; }
            public int Y10 { get; set; }
            public int X100 {  get; set; }
            public int Y100 { get; set; }

            public int X2 { get; set; }
            public int Y2 { get; set; }
            public int X3 { get; set; }
            public int Y3 { get; set; }
            public int X4 { get; set; }
            public int Y4 { get; set; }

        }
        public void Configure()
        {
            int trash = 0;
            int x = 0;
            int y = 0;
            int startColorR = 0;
            int startColorG = 0;
            int startColorB = 0;

            int x1 = 0;
            int y1 = 0;

            int cl1 = 0;
            int cl2 = 0;

            int X1 = 0;
            int Y1 = 0;
            int X10 = 0;
            int Y10 = 0;
            int X100 = 0;
            int Y100 = 0;

            int X2 = 0;
            int Y2 = 0;
            int X3 = 0;
            int Y3 = 0;
            int X4 = 0;
            int Y4 = 0;



            if (!File.Exists(ConfigFilePath))
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("You need to setup program, please follow instructions");
                Console.WriteLine("Tap any Key...");
                Console.ReadKey();
                Console.ResetColor();
                Console.Clear();
                Thread.Sleep(500);
                GetCoordinatesAndColors(ref x, ref y, ref startColorR, ref startColorG, ref startColorB,
                    "1 Move your cursor slightly to the left of the \"Rolling in\" text in the countdown indicator when indicator is black.\n" +
                    "2 Press <Q> key.\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);
                GetCoordinates(ref x1, ref y1,
                    "\"1 Move your cursor slightly to the left of the last number in the row where are numbers in circles (this zone must be red, black or green).\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);
                GetCoordinates(ref cl1, ref cl2,
                    "\"1 Move your cursor to the \"Clear\" Button.\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);
                GetCoordinates(ref X1, ref Y1,
                    "\"1 Move your cursor to the \"+1\" Button.\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);
                GetCoordinates(ref X10, ref Y10,
                    "\"1 Move your cursor to the \"+10\" Button.\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);
                GetCoordinates(ref X100, ref Y100,
                    "\"1 Move your cursor to the \"+100\" Button.\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);
                GetCoordinates(ref X2, ref Y2,
                    "\"1 Move your cursor to the big \"Red\" bet Button.\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);               
                GetCoordinates(ref X3, ref Y3,
                    "\"1 Move your cursor to the big \"Black\" bet Button.\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);
                GetCoordinates(ref X4, ref Y4,
                    "\"1 Move your cursor to the big \"Green\" bet Button.\n" +
                    "2 Press <Q> key\n" +
                    "3 Press the Enter key to save the value.\n" +
                    "For help go to readme.txt.");
                Console.WriteLine("Saved!\n");
                Thread.Sleep(500);               
                
                Coordinates config = new Coordinates()
                {
                    x = x,
                    y = y,
                    startColorR = startColorR,
                    startColorG = startColorG,
                    startColorB = startColorB,

                    x1 = x1,
                    y1 = y1,

                    cl1 = cl1,
                    cl2 = cl2,

                    X1 = X1,
                    Y1 = Y1,
                    X10 = X10,
                    Y10 = Y10,
                    X100 = X100,
                    Y100 = Y100,

                    X2 = X2,
                    Y2 = Y2,
                    X3 = X3,
                    Y3 = Y3,
                    X4 = X4,
                    Y4 = Y4,
                };
                BotConfiguration.JsonToFile(config, ConfigFilePath);
                Console.Clear();
            }
            
            Coordinates Config = Read(ConfigFilePath);
            MainLogic.x = Config.x;
            MainLogic.y = Config.y;

            MainLogic.startColorR = Config.startColorR;
            MainLogic.startColorG = Config.startColorG;
            MainLogic.startColorB = Config.startColorB;

            MainLogic.x1 = Config.x1;
            MainLogic.y1 = Config.y1;

            MainLogic.cl1 = Config.cl1;
            MainLogic.cl2 = Config.cl2;

            MainLogic.X1 = Config.X1;
            MainLogic.Y1 = Config.Y1;
            MainLogic.X10 = Config.X10;
            MainLogic.Y10 = Config.Y10;
            MainLogic.X100 = Config.X100;
            MainLogic.Y100 = Config.Y100;

            MainLogic.X2 = Config.X2;
            MainLogic.Y2 = Config.Y2;
            MainLogic.X3 = Config.X3;
            MainLogic.Y3 = Config.Y3;
            MainLogic.X4 = Config.X4;
            MainLogic.Y4 = Config.Y4;

        }
        private static Coordinates Read(string path)
        {
            string json = File.ReadAllText(path);
            var Object = JsonConvert.DeserializeObject<Coordinates>(json);
            return Object;
        }

        static void GetCoordinatesAndColors(ref int x, ref int y, ref int colorR, ref int colorG, ref int colorB, string message)
        {
            Console.WriteLine(message);

            while (true)
            {
                if (GetAsyncKeyState(VK_Q) < 0)
                {
                    Point cursorPos;
                    GetCursorPos(out cursorPos);
                    Color color = GetColorAt(cursorPos.X, cursorPos.Y);

                    x = cursorPos.X;
                    y = cursorPos.Y;
                    colorR = color.R;
                    colorG = color.G;
                    colorB = color.B;
                    Console.WriteLine($"Color at ({cursorPos.X}, {cursorPos.Y}): R={color.R}, G={color.G}, B={color.B}");


                    Thread.Sleep(200); // Debounce to avoid multiple detections for a single click
                }

                if (GetAsyncKeyState(VK_ENTER) < 0) // Escape key
                {

                    break;
                }

                Thread.Sleep(10); 
            }
        }
        static void GetCoordinates(ref int x, ref int y, string message)
        {
            Console.WriteLine(message);

            while (true)
            {
                if (GetAsyncKeyState(VK_Q) < 0)
                {
                    Point cursorPos;
                    GetCursorPos(out cursorPos);
                   
                    x = cursorPos.X;
                    y = cursorPos.Y;                    
                    Console.WriteLine($"Coordinates at ({cursorPos.X}, {cursorPos.Y})");


                    Thread.Sleep(200); // Debounce to avoid multiple detections for a single click
                }

                if (GetAsyncKeyState(VK_ENTER) < 0) // Escape key
                {

                    break;
                }

                Thread.Sleep(10); 
            }
        }

        private static Color GetColorAt(int x, int y)
        {
            IntPtr hdc = GetDC(IntPtr.Zero);
            uint pixel = GetPixel(hdc, x, y);
            ReleaseDC(IntPtr.Zero, hdc);

            Color color = Color.FromArgb((int)(pixel & 0x000000FF),
                                         (int)(pixel & 0x0000FF00) >> 8,
                                         (int)(pixel & 0x00FF0000) >> 16);
            return color;
        }
    }
}

