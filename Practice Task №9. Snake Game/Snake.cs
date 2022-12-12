using System.Drawing;
using static Practice_Task__9._Snake_Game.Program;

namespace Practice_Task__9._Snake_Game
{
    public class Snake
    {
        public class Part
        {
            public int x, y, x_old, y_old;
        }
        public static ConsoleKeyInfo key;

        public static bool isStarted;
        public static Snake snake;

        public enum move { up, down, left, right, stop }
        public static move dir = move.stop;

        public int HeadX, HeadY;
        public List<Part> Parts = new List<Part>();

        public static int foodX = 0, foodY = 0;

        public static int points = 0;

        public static void Start()
        {
            isStarted = true;

            snake = new Snake() { HeadX = (int)Border.Right_Border / 2, HeadY = (int)Border.Top / 2, Parts = new List<Snake.Part>() { new Snake.Part() { x = ((int)Border.Right_Border / 2) - 1, y = (int)Border.Top / 2, x_old = ((int)Border.Right_Border / 2) - 1, y_old = (int)Border.Top / 2 } } };
            Borders();

            Thread thread = new Thread(new ThreadStart(Go));
            thread.Start();

        }
        private static void Borders()
        {
            for (int y = 0; y < (int)Border.Top; y++)
            {
                for (int x = 0; x < (int)Border.Right_Border; x++)
                {
                    Console.SetCursorPosition(x, y);
                    if (y == 0 || y == (int)Border.Top - 1)
                    {
                        Console.Write("=");
                        continue;
                    }
                    if (x == 0 || x == (int)Border.Right_Border - 1)
                    {
                        Console.Write("|");
                    }
                    else
                    {
                        Console.Write(" ");
                    }
                }
                Console.Write("\n");
            }
            TimeTo_Eat();
        }

        private static void Go()
        {
            while (isStarted)
            {
                Draw_Snake();
                Input();
                Logics();
                Thread.Sleep(77);
            }
            Console.SetCursorPosition((int)Border.Right_Border / 2, (int)Border.Top / 2);
            Console.Write("Стоп!");
            Console.SetCursorPosition((int)Border.Right_Border + 2, (int)Border.Top / 2);
            Console.Write("Конец игры. ENTER для повтора");
            while (true)
            {
                if (Console.ReadKey(true).Key == ConsoleKey.Enter)
                {
                    Console.Clear();
                    Start();
                    break;
                }
            }
        }
        private static void Draw_Snake()
        {
            Console.SetCursorPosition(snake.HeadX, snake.HeadY);
            Console.Write("*");

            for (int i = 0; i < snake.Parts.Count(); i++)
            {
                Console.SetCursorPosition(snake.Parts[i].x_old, snake.Parts[i].y_old);
                Console.Write(" ");
                Console.SetCursorPosition(snake.Parts[i].x, snake.Parts[i].y);
                Console.Write("*");
            }
            Console.SetCursorPosition(foodX, foodY);
            Console.Write("+");
        }
        private static void TimeTo_Eat()
        {
            Random random = new Random();

            foodX = random.Next(3, (int)Border.Right_Border) - 2;
            foodY = random.Next(3, (int)Border.Top - 2);
        }
        private static void Input()
        {
            if (Console.KeyAvailable)
            {
                key = Console.ReadKey(true);
                if (key.Key == ConsoleKey.UpArrow || key.Key == ConsoleKey.W)
                {
                    if (dir != move.down)
                    {
                        dir = move.up;
                    }
                }
                if (key.Key == ConsoleKey.DownArrow || key.Key == ConsoleKey.S)
                {
                    if (dir != move.up)
                    {
                        dir = move.down;
                    }
                }
                if (key.Key == ConsoleKey.LeftArrow || key.Key == ConsoleKey.A)
                {
                    if (dir != move.right)
                    {
                        dir = move.left;
                    }
                }
                if (key.Key == ConsoleKey.RightArrow || key.Key == ConsoleKey.D)
                {
                    if (dir != move.left)
                    {
                        dir = move.right;
                    }
                }
            }
        }
        private static void Logics()
        {
            int X_Old = snake.HeadX, Y_Old = snake.HeadY;
            if (dir == move.up)
            {
                snake.HeadY--;
            }
            if (dir == move.down)
            {
                snake.HeadY++;
            }
            if (dir == move.right)
            {
                snake.HeadX++;
            }
            if (dir == move.left)
            {
                snake.HeadX--;
            }

            if (dir != move.stop)
            {
                foreach (var part in snake.Parts) // СТОЛКНОВЕНИЕ ЗМЕИ С САМОЙ СОБОЙ
                {
                    if (part.x == snake.HeadX && part.y == snake.HeadY)
                    {
                        isStarted = false;
                    }
                }
                if (snake.HeadX == 0 || snake.HeadX == (int)Border.Right_Border || snake.HeadY == 0 || snake.HeadY == (int)Border.Top - 1)
                {
                    isStarted = false;
                }

                for (int i = 0; i < snake.Parts.Count(); i++)
                {
                    snake.Parts[i].x_old = snake.Parts[i].x;
                    snake.Parts[i].y_old = snake.Parts[i].y;

                    if (i == 0)
                    {
                        snake.Parts[i].x = X_Old;
                        snake.Parts[i].y = Y_Old;
                        continue;
                    }

                    snake.Parts[i].x = snake.Parts[i - 1].x_old;
                    snake.Parts[i].y = snake.Parts[i - 1].y_old;
                }

                Console.SetCursorPosition(17, (int)Border.Top);
                Console.Write($"Score - {points}");

                if (snake.HeadX == foodX && snake.HeadY == foodY)
                {
                    snake.Parts.Add(new Snake.Part() { x = snake.Parts[snake.Parts.Count() - 1].x_old, y = snake.Parts[snake.Parts.Count() - 1].y_old });
                    points++;
                    TimeTo_Eat();
                }
            }
        }
    }
}
