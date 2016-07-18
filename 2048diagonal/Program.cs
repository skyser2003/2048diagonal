using System;

namespace _2048diagonal {
    class Program {
        static void Print(Block[,] map)
        {
            int width = map.GetLength(0);
            int height = map.GetLength(1);

            for (int j = height - 1; j >= 0; --j) {
                for (int i = 0; i < width; ++i) {
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

            Console.WriteLine();
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
                var key = Console.ReadKey(true).Key;

                int x = 0;
                int y = 0;

                if (key == ConsoleKey.W) {
                    y = 1;
                }
                else if (key == ConsoleKey.A) {
                    x = -1;
                }
                else if (key == ConsoleKey.S) {
                    y = -1;
                }
                else if (key == ConsoleKey.D) {
                    x = 1;
                }
                else if (key == ConsoleKey.Q) {
                    x = -1;
                    y = 1;
                }
                else if (key == ConsoleKey.E) {
                    x = 1;
                    y = 1;
                }
                else if (key == ConsoleKey.Z) {
                    x = -1;
                    y = -1;
                }
                else if (key == ConsoleKey.C) {
                    x = 1;
                    y = -1;
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