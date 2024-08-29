using System.Drawing;
using System.Runtime.InteropServices;
using static GoDota2_Bot.ConfigureZones;



namespace GoDota2_Bot
{
    internal class MainLogic
    {
        [DllImport("user32.dll")]
        static extern IntPtr GetDC(IntPtr hwnd);

        [DllImport("user32.dll")]
        static extern int ReleaseDC(IntPtr hwnd, IntPtr hdc);

        [DllImport("gdi32.dll")]
        static extern uint GetPixel(IntPtr hdc, int nXPos, int nYPos);

        [DllImport("user32.dll")]
        static extern bool SetCursorPos(int X, int Y);

        [DllImport("user32.dll")]
        public static extern void mouse_event(int dwFlags, int dx, int dy, int cButtons, int dwExtraInfo);

        public const int MOUSEEVENTF_LEFTDOWN = 0x02;
        public const int MOUSEEVENTF_LEFTUP = 0x04;

        static List<string> colorsHistory = new List<string>();

        Coordinates coordinates = new Coordinates();

        public static int x;
        public static int y;

        public static int startColorR;
        public static int startColorG;
        public static int startColorB; 

        public static int x1;
        public static int y1;

        public static int cl1;
        public static int cl2;

        public static int X1;
        public static int Y1;

        public static int X10;
        public static int Y10;

        public static int X100;
        public static int Y100;

        public static int X2;
        public static int Y2;

        public static int X3;
        public static int Y3;

        public static int X4;
        public static int Y4;

        static int addedPointsR = 0;
        static int addedPointsB = 0;
        static int addedPointsG = 0;
        static int addedPointsRedStrike = 0;
        static int addedPointsBlackStrike = 0;

        public static string currentColor = "?";

        static int profit = 0;

        static int lostPointsR = 0;
        static int lostPointsB = 0;
        static int lostPointsG = 0;
        static int lostPointsRedStrike = 0;
        static int lostPointsBlackStrike = 0;

        static bool red = true;
        static bool black = false;

        public static double redCount;
        public static double blackCount;
        public static double greenCount;

        public static double redProbab;
        public static double blackProbab;
        public static double greenProbab;

        static int redStrike = 0;
        static int blackStrike = 0;

        public static int notGreen = 0;
        public static int notRed = 0;
        public static int notBlack = 0;

        static bool redNow = false;
        static bool blackNow = false;

        public static bool bettingGreen = false;
        public static bool bettingRed = false;
        public static bool bettingBlack = false;

        static int greenStrike = 0;

        public static int round;

        static int i;

        public static bool pause = true;

        //Fill in

        public static int startBalance;
        public static int currentBalance;

        public static int greenLimit = 30;
        public static int redLimit = 5;
        public static int blackLimit = 5;

        public static int minProfitGreen = 1;
        public static int minProfitRed = 1;
        public static int minProfitBlack = 1;


        public void AutoBet()
        {
            
            Thread.Sleep(25000);

            Start();
            BetCheck();           
            CheckStrike();
            BalanceDifference();
            
        }

        void Start()
        {
            
            Console.WriteLine(" ");
            Console.WriteLine("Start running...");
            Thread.Sleep(10);

            while (true)
            {

                Color startColor = GetColorAt(x, y);

                //SetCursorPos(x, y);
                
                if (startColor.R == startColorR && startColor.G == startColorG && startColor.B == startColorB)
                {
                    round++;
                    Console.WriteLine("Rounds:" + round);

                    Thread.Sleep(10);


                    Color color0 = GetColorAt(x1, y1);
                    //SetCursorPos(x1, y1);

                    if (color0.R == 76 && color0.G == 175 && color0.B == 80)
                    {
                        currentColor = "green";
                        greenCount++;

                        notGreen = 0;
                        notRed++;
                        notBlack++;

                    }
                    else if (color0.R == 176 && color0.G == 74 && color0.B == 67)
                    {
                        currentColor = "red";
                        redCount++;

                        notRed = 0;
                        notGreen++;
                        Console.WriteLine("Green not detected: " + notGreen + " times");
                        notBlack++;

                    }
                    else if (color0.R == 28 && color0.G == 29 && color0.B == 36)
                    {
                        currentColor = "black";
                        blackCount++;

                        notBlack = 0;
                        notGreen++;
                        Console.WriteLine("Green not detected: " + notGreen + " times");
                        notRed++;

                    }
                    else
                    {
                        currentColor = "not found";
                    }

                    Console.WriteLine("Current Color:" + currentColor);

                    colorsHistory.Insert(0, currentColor);
                    //Console.WriteLine(string.Join(", ", colorsHistory));

                    MathLogic();

                    Console.WriteLine("redCount: " + redProbab /*+ "/46.66 %"*/ + "     " + redCount);
                    Console.WriteLine("blackCount: " + blackProbab /*+ "/46.66 %"*/ + "   " + blackCount);
                    Console.WriteLine("greenCount: " + greenProbab /*+ "/6.66 %"*/ + "   " + greenCount);



                    return;
                }
                Thread.Sleep(2000);


            }

        }
        static void CheckStrike()
        {    
            if (!pause)
            {
                return;
            }
            int betCount = 0; 
            if (notGreen >= greenLimit && betCount < 2)
            {
                BetGreen();
                betCount++; 
            }
            if (notRed >= redLimit && notBlack >= blackLimit)
            {
                CountComparison();
            }
            else
            {
                if (notRed >= redLimit && betCount < 2)
                {
                    BetRed();
                    betCount++;
                }
                if (notBlack >= blackLimit && betCount < 2)
                {
                    BetBlack();
                    betCount++;
                }
            }
        }
        public static void CountComparison()
        {
            if(bettingBlack)
            {
                BetBlack();
            }
            else if (bettingRed)
            {
                BetRed();
            }
            else
            {
                if (redCount > blackCount)
                {
                    BetBlack();
                }
                else if (blackCount > redCount)
                {
                    BetRed();
                }
                else
                {
                    Random random = new Random();
                    int randomChoice = random.Next(1, 3);
                    if (randomChoice == 1)
                    {
                        BetRed();
                    }
                    else
                    {
                        BetBlack();
                    }
                }
            }            
        }

        static void BetCheck()
        {
            if (bettingGreen == true && currentColor == "green")
            {
                currentBalance += addedPointsG * 14;
                bettingGreen = false;
                lostPointsG = 0;
                addedPointsG = 0;
            }
            else if (bettingRed == true && currentColor == "red")
            {
                currentBalance += addedPointsR * 2;
                bettingRed = false;
                lostPointsR = 0;
                addedPointsR = 0;
            }
            else if (bettingBlack == true && currentColor == "black")
            {
                currentBalance += addedPointsB * 2;
                bettingBlack = false;
                lostPointsB = 0;
                addedPointsB = 0;
            }
        }

        static void BetGreen()
        {
            Console.WriteLine("GreenBet running...");

            bettingGreen = true;

            Clear();
            ProfitCountGreen();
            SetPriceG();

            Thread.Sleep(200);

            SetCursorPos(X4, Y4);
            mouse_event(MOUSEEVENTF_LEFTDOWN, X4, Y4, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, X4, Y4, 0, 0);

            Thread.Sleep(3000);
        }

        static void BetRed()
        {
            Console.WriteLine("BlackBet running...");

            bettingRed = true;

            Clear();
            ProfitCountRed();
            SetPriceR();

            Thread.Sleep(200);

            SetCursorPos(X2, Y2);
            mouse_event(MOUSEEVENTF_LEFTDOWN, X2, Y2, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, X2, Y2, 0, 0);

            Thread.Sleep(3000);

        }

        static void BetBlack()
        {
            Console.WriteLine("BlackBet running...");

            bettingBlack = true;

            Clear();
            ProfitCountBlack();
            SetPriceB();

            Thread.Sleep(200);

            SetCursorPos(X3, Y3);
            mouse_event(MOUSEEVENTF_LEFTDOWN, X3, Y3, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, X3, Y3, 0, 0);

            Thread.Sleep(3000);
        }

























        static void ProfitCountGreen()
        {            
            ProfitCount("green", ref addedPointsG, ref lostPointsG, 14, minProfitGreen);
        }

        static void ProfitCountRed()
        {            
            ProfitCount("red", ref addedPointsR, ref lostPointsR, 2, minProfitRed);
        }

        static void ProfitCountBlack()
        {            
            ProfitCount("black", ref addedPointsB, ref lostPointsB, 2, minProfitBlack);
        }
        
        static void ProfitCount(string color, ref int addedPoints, ref int lostPoints, int multiplier, int minProfit)
        {
            Console.WriteLine($"profitCount {color} running...");

            while (true)
            {
                int profit = (addedPoints * multiplier) - (lostPoints + addedPoints);

                if (profit >= minProfit)
                {
                    break;
                }
                addedPoints++;
            }

            currentBalance -= addedPoints;
            lostPoints += addedPoints;
            StopChecker();
        }


        static void SetPriceG()
        {
            Console.WriteLine("SetPriceG running...");
            AddPoints(addedPointsG);
            Console.WriteLine("addedPointsG:" + addedPointsG);           
        }
        static void SetPriceR()
        {
            Console.WriteLine("SetPrice red running...");
            AddPoints(addedPointsR);
            Console.WriteLine("added points red:" + addedPointsR);
        }
        static void SetPriceB()
        {
            Console.WriteLine("SetPrice black running...");
            AddPoints(addedPointsB);                       
            Console.WriteLine("added points black:" + addedPointsB);
        }
        static void AddPoints(int points)
        {
            while (points > 0)
            {
                if (points >= 100)
                {
                    SetCursorPos(X100, Y100);                    
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X100, Y100, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, X100, Y100, 0, 0);

                    points -= 100;
                }
                else if (points >= 10)
                {
                    SetCursorPos(X10, Y10);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X10, Y10, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, X10, Y10, 0, 0);

                    points -= 10;  
                }
                else
                {                   
                    SetCursorPos(X1, Y1);
                    mouse_event(MOUSEEVENTF_LEFTDOWN, X1, Y1, 0, 0);
                    mouse_event(MOUSEEVENTF_LEFTUP, X1, Y1, 0, 0);

                    points--; 
                }                
                Thread.Sleep(10);
            }
        }

        static void Clear()
        {
            Console.WriteLine("Clear running...");

            SetCursorPos(cl1, cl2);
            mouse_event(MOUSEEVENTF_LEFTDOWN, cl1, cl2, 0, 0);
            mouse_event(MOUSEEVENTF_LEFTUP, cl1, cl2, 0, 0);

            Thread.Sleep(200);
        }

        static void StopChecker()
        {
            if (currentBalance < 0) { Console.ReadKey(); }
        }

        static void MathLogic()
        {
            redProbab = (redCount / round) * 100;
            blackProbab = (blackCount / round) * 100;
            greenProbab = (greenCount / round) * 100;

            redProbab = (redCount / round) / (7d / 15d);
            blackProbab = (blackCount / round) / (7d / 15d);
            greenProbab = (greenCount / round) / (1d / 15d);

            redProbab = Math.Round(redProbab, 2);
            blackProbab = Math.Round(blackProbab, 2);
            greenProbab = Math.Round(greenProbab, 2);
        }
        public static string BalanceDifference()
        {
            int differenceBalance = currentBalance - startBalance;
            string plusOrMinus = "";
            string message = "";
            if (differenceBalance > 0)
            {
                plusOrMinus = "+";
            }
            if (differenceBalance == 0)
            {
                message = $"Balance: {currentBalance}";
            }
            else
            {
                message = $"Balance: {currentBalance}({plusOrMinus}{differenceBalance})";
            }
            
            Console.WriteLine(message);
            return message;
        }










        static Color GetColorAt(int x, int y)
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
