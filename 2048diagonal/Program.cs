using System;

namespace _2048diagonal {
    class Program {
        static void Print(Block[,] map)
        {
            for (int i = 0; i < map.GetLength(0); ++i) {
                for (int j = 0; j < map.GetLength(1); ++j) {
                    if (map[i, j] == null) {
                        Console.Write("0 ");
                    }
                    else {
                        Console.Write(map[i, j].Value);
                        Console.Write(" ");
                    }
                }

                Console.WriteLine();
            }
        }

        static void Main(string[] args)
        {
            int Width = 4;
            int Height = 4;

            var logic = new Logic();
            logic.Init(Width, Height);

            var map = logic.Map;

            Print(map);

            while (true) {
                if (Console.KeyAvailable == false) {
                    continue;
                }

                int x = 0;
                int y = 0;

                if (Console.ReadKey().Key == ConsoleKey.W) {
                    y = 1;
                }
                else if (Console.ReadKey().Key == ConsoleKey.A) {
                    x = -1;
                }
                else if (Console.ReadKey().Key == ConsoleKey.S) {
                    y = -1;
                }
                else if (Console.ReadKey().Key == ConsoleKey.D) {
                    x = 1;
                }

                if (x == 0 && y == 0) {
                    continue;
                }

                logic.Move(x, y);

                Print(map);
            }
        }
    }
}