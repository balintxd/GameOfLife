using System;
using System.Drawing.Drawing2D;
using System.IO;
using System.Linq;

namespace gameoflife
{
    class Program
    {
        static int Height = Console.WindowHeight - 1;
        static int Width = Console.WindowWidth - 1;

        static bool[,] firstCells = new bool[Height, Width];
        static bool[,] cells = new bool[Height, Width];
        static bool[,] cellsNext = new bool[Height, Width];

        static int generation = 0;


        static void Main(string[] args)
        {
            Build();
            Array.Copy(cells, firstCells, Height * Width);

            while (true)
            {
                Draw();
                Grow();

                System.Threading.Thread.Sleep(100);
                generation++;
            }
        }

        static void Build()
        {
            ConsoleKey key;
            int left = 0, top = 0;

            Console.Clear();
            Console.SetCursorPosition(0, 0);

            do
            {
                key = Console.ReadKey(true).Key;

                switch (key)
                {
                    case ConsoleKey.W:
                        if (top == 0)
                        {
                            Console.SetCursorPosition(left, Height - 1);
                            top = Height - 1;
                        }
                        else
                        {
                            Console.SetCursorPosition(left, top - 1);
                            top--;
                        }
                        break;
                    case ConsoleKey.A:
                        if (left == 0)
                        {
                            Console.SetCursorPosition(Width - 1, top);
                            left = Width - 1;
                        }
                        else
                        {
                            Console.SetCursorPosition(left - 1, top);
                            left--;
                        }
                        break;
                    case ConsoleKey.S:
                        if (top == Height - 1)
                        {
                            Console.SetCursorPosition(left, 0);
                            top = 0;
                        }
                        else
                        {
                            Console.SetCursorPosition(left, top + 1);
                            top++;
                        }
                        break;
                    case ConsoleKey.D:
                        if (left == Width - 1)
                        {
                            Console.SetCursorPosition(0, top);
                            left = 0;
                        }
                        else
                        {
                            Console.SetCursorPosition(left + 1, top);
                            left++;
                        }
                        break;
                    case ConsoleKey.Spacebar:
                        if (!cells[top, left])
                        {
                            Console.Write("O");
                            cells[top, left] = true;
                        }
                        else
                        {
                            Console.Write(" ");
                            cells[top, left] = false;
                        }
                        Console.SetCursorPosition(left, top);
                        break;
                    default:
                        break;
                }

            } while (key != ConsoleKey.Enter);
        }

        static void Draw()
        {
            Console.Clear();

            Console.Title = "Gen: " + generation;

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    if (cells[y, x])
                    {
                        Console.SetCursorPosition(x, y);
                        Console.Write("X");
                    }
                }
            }
        }

        static void Grow()
        {
            int near;

            Array.Copy(cells, cellsNext, Height * Width);

            for (int y = 0; y < Height; y++)
            {
                for (int x = 0; x < Width; x++)
                {
                    near = GetNear(y, x);

                    if (cells[y, x])
                    {
                        if (near < 2) cellsNext[y, x] = false;
                        if (near > 3) cellsNext[y, x] = false;
                    }
                    else
                    {
                        if (near == 3)
                        {
                            cellsNext[y, x] = true;
                        }
                    }
                }
            }

            bool same = true;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    if (!cells[i, j] == cellsNext[i, j]) same = false;
                }
            }
            if (same) End();

            Array.Copy(cellsNext, cells, Height * Width);
        }

        static int GetNear(int y, int x)
        {
            int near = 0;

            for (int i = y - 1; i < y + 2; i++)
            {
                for (int j = x - 1; j < x + 2; j++)
                {
                    if (i >= 0 && i < Height && j >= 0 && j < Width && !(i == y && j == x))
                    {
                        if (cells[i, j]) near++;
                    }
                }
            }

            return near;
        }

        static void End()
        {
            string seed = null;
            for (int i = 0; i < Height; i++)
            {
                for (int j = 0; j < Width; j++)
                {
                    seed += Convert.ToByte(firstCells[i, j]);
                }
            }

            using (StreamWriter sw = new StreamWriter(@"log.txt", true))
            {
                sw.WriteLine($"{generation};{seed}");
            }

            Console.Clear();
            Console.WriteLine("end");
            Console.ReadKey();
            System.Environment.Exit(0);
        }
    }
}