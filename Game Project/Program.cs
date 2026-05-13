using System.Runtime.CompilerServices;

namespace Game_Project
{
    internal class Program
    {
        static int MeX = 10;
        static int MeY = 10;
        static int highScore = 0;
        
        
        static int targetCount = 20;
        static int[] targetX = new int[targetCount];
        static int[] targetY = new int[targetCount];
        static int[] targetDirectionX = new int[targetCount];
        static int[] targetDirectionY = new int[targetCount];

        
        static int score = 0;
        static int targetSpeed = 300; 
        static bool running = true;

        
        static DateTime gameStart;
        static int gameSeconds = 60; //the timer til the games end 

        static void Main()
        {
            Console.CursorVisible = false;

            while (true)
            {
                int choice = Menu();
                if (choice == 1)
                    Game();
                else
                    Instructions();
            }
        }

        
        static int Menu() //diplaying the menu with options
        {
            Console.Clear();
            int centerX = Console.WindowWidth / 2;

            Console.ForegroundColor = ConsoleColor.Magenta;

            Console.SetCursorPosition(centerX - 17, 5);
            Console.WriteLine("=================================");
            Console.SetCursorPosition(centerX - 17, 6);
            Console.WriteLine("   RAMEEN'S MOVING TARGET GAME ");
            Console.SetCursorPosition(centerX - 17, 7);
            Console.WriteLine("=================================");

            Console.SetCursorPosition(centerX - 6, 9);
            Console.WriteLine("1 - Start Game");

            Console.SetCursorPosition(centerX - 7, 10);
            Console.WriteLine(" 2 - Instructions");

            Console.SetCursorPosition(centerX - 7, 12);
            Console.ForegroundColor = ConsoleColor.White;
            Console.Write("  Choose option: ");

            while (true)
            {
                string input = Console.ReadLine();

                if (input == "1" || input == "2")
                    return int.Parse(input);

                Console.SetCursorPosition(centerX - 7, 14);
                Console.Write("Enter 1 or 2");
            }
        }

        
        static void Instructions() // display the instructions of the game 
        {
            Console.Clear();
            Console.ForegroundColor = ConsoleColor.Cyan;

            int centerX = Console.WindowWidth / 2;
            int y = 5;

            Console.SetCursorPosition(centerX - 7, y++);
            Console.WriteLine("INSTRUCTIONS:");
            y++;

            Console.SetCursorPosition(centerX - 20, y++);
            Console.WriteLine("       Move your X with the arrow keys.");

            Console.SetCursorPosition(centerX - 35, y++);
            Console.WriteLine("              Your goal is to catch red T targets to score points.");

            Console.SetCursorPosition(centerX - 22, y++);
            Console.WriteLine("           Targets get faster each time.");

            Console.SetCursorPosition(centerX - 10, y++);
            Console.WriteLine("   Game lasts 1 minute.");

            y += 2;
            Console.SetCursorPosition(centerX - 12, y++);
            Console.WriteLine("    Press any key to return...");

            Console.ReadKey(true);
        }

       
        static void Game()
        {
            Console.Clear();
            score = 0;
            targetSpeed = 300; 

            SetupTargets();
            MeX = 10;
            MeY = 5;
            DrawBorder();
            gameStart = DateTime.Now;
            running = true;

            while (running)
            {
                HandleInput();       
                MoveTargets();
                CheckCatch();
                DrawHUD();
                DrawObjects();

                System.Threading.Thread.Sleep(targetSpeed);

                if ((DateTime.Now - gameStart).TotalSeconds >= gameSeconds)
                    running = false;
            }

            EndScreen();
        }

        static void HandleInput()
        {
            while (Console.KeyAvailable)
            {
                ConsoleKey k = Console.ReadKey(true).Key;
                int oldX = MeX;
                int oldY = MeY;

                
                if (k == ConsoleKey.LeftArrow) MeX -= 4;
                if (k == ConsoleKey.RightArrow) MeX += 4;
                if (k == ConsoleKey.UpArrow) MeY -= 2;
                if (k == ConsoleKey.DownArrow) MeY += 2;

                
                if (MeX < 1) MeX = 1;
                if (MeX > Console.WindowWidth - 2) MeX = Console.WindowWidth - 2;
                if (MeY < 4) MeY = 4;
                if (MeY > Console.WindowHeight - 2) MeY = Console.WindowHeight - 2;

               
                Console.SetCursorPosition(oldX, oldY);
                Console.Write(" ");
            }
        }

       
        static void SetupTargets()
        {
            Random r = new Random();

            for (int i = 0; i < targetCount; i++)
            {
                targetX[i] = r.Next(2, Console.WindowWidth - 2);
                targetY[i] = r.Next(5, Console.WindowHeight - 2);

                targetDirectionX[i] = r.Next(-1, 2);
                targetDirectionY[i] = r.Next(-1, 2);

                if (targetDirectionX[i] == 0 && targetDirectionY[i] == 0)
                    targetDirectionX[i] = 1;
            }
        }

      
        static void MoveTargets()
        {
            for (int i = 0; i < targetCount; i++)
            {
                int oldX = targetX[i];
                int oldY = targetY[i];

                targetX[i] += targetDirectionX[i];
                targetY[i] += targetDirectionY[i];

                if (targetX[i] <= 1 || targetX[i] >= Console.WindowWidth - 2)
                    targetDirectionX[i] *= -1;

                if (targetY[i] <= 4 || targetY[i] >= Console.WindowHeight - 2)
                    targetDirectionY[i] *= -1;

                // erase old position
                Console.SetCursorPosition(oldX, oldY);
                Console.Write(" ");
            }
        }

       
        static void CheckCatch() 
        {
            Random r = new Random();

            for (int i = 0; i < targetCount; i++)
            {
                             
                bool touchingX = Math.Abs(targetX[i] - MeX) <= 1;
                bool touchingY = Math.Abs(targetY[i] - MeY) <= 1;

                if (touchingX && touchingY)
                {
                    
                    score = score + 5; //update the score and adds 5 

                    
                    if (targetSpeed > 100)
                    {
                        targetSpeed = targetSpeed - 10;
                    }

                    targetX[i] = r.Next(2, Console.WindowWidth - 2); //this will move the target to a new random spot
                    
                    targetY[i] = r.Next(5, Console.WindowHeight - 2);
                }
            }
        }

       
        static void DrawBorder()
        {
            for (int x = 0; x < Console.WindowWidth; x++)
            {
                Console.SetCursorPosition(x, 3);
                Console.Write("-");
                Console.SetCursorPosition(x, Console.WindowHeight - 1);
                Console.Write("-");
            }
        }

       
        static void DrawHUD()
        {
            Console.ForegroundColor = ConsoleColor.Magenta;
            Console.SetCursorPosition(1, 1);
            
            int timePassed = (int)(DateTime.Now - gameStart).TotalSeconds;
            int timeLeft = gameSeconds - timePassed;

            Console.Write("Score: " + score + "   Time: " + timeLeft + "s");
        }

      
        static void DrawObjects()
        {
           
            Console.ForegroundColor = ConsoleColor.Cyan;
            Console.SetCursorPosition(MeX, MeY);
            Console.Write("X");

            
            Console.ForegroundColor = ConsoleColor.Red;
            for (int i = 0; i < targetCount; i++)
            {
                Console.SetCursorPosition(targetX[i], targetY[i]);
                Console.Write("T");
            }
        }

        
        static void EndScreen()
        {
            Console.Clear();

            // Update high score
            if (score > highScore)
                highScore = score;

            int centerX = Console.WindowWidth / 2;
            int y = Console.WindowHeight / 2 - 3;

            
            Console.ForegroundColor = ConsoleColor.Red; //When it is game over
            Console.SetCursorPosition(centerX - 5, y++);
            Console.WriteLine("GAME OVER!");

            
            Console.ForegroundColor = ConsoleColor.Yellow; //when it is final score
            Console.SetCursorPosition(centerX - ("Final Score: " + score).Length / 2, y++);
            Console.WriteLine("Final Score: " + score);

            
            Console.ForegroundColor = ConsoleColor.Green; // when it is highscore
            Console.SetCursorPosition(centerX - ("High Score: " + highScore).Length / 2, y++);
            Console.WriteLine("High Score: " + highScore);

            
            Console.ForegroundColor = ConsoleColor.White;
            y += 2;
            Console.SetCursorPosition(centerX - 14, y);
            Console.WriteLine("Press any key to return...");

            Console.ReadKey(true);
        }
    }

     
}
