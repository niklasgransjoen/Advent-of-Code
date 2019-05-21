using AOC.Resources;
using System;
using System.Text;
using System.Threading.Tasks;

namespace day_10a
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            _ = Task.Run(async () =>
            {
                string[] input = await General.GetInputFromRelativePath("day10.txt");
                Light[] lights = ParseInput(input);

                await DoStuff(lights);

                return 0;
            }).Result;

            Console.ReadKey();
        }

        private static Light[] ParseInput(string[] input)
        {
            Light[] lights = new Light[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i];

                Point pos = new Point
                {
                    x = int.Parse(line.Substring(10, 6)),
                    y = int.Parse(line.Substring(18, 6))
                };

                Point vel = new Point
                {
                    x = int.Parse(line.Substring(36, 2)),
                    y = int.Parse(line.Substring(40, 2))
                };

                lights[i] = new Light(pos, vel);
            }

            return lights;
        }

        private static async Task DoStuff(Light[] lights)
        {
            for (int i = 0; i < 1000; i++)
            {
                char[,] characters = new char[1000, 1000];
                for (int j = 0; j < characters.GetLength(0); j++)
                    for (int k = 0; k < characters.GetLength(1); k++)
                        characters[j, k] = '.';

                foreach (Light light in lights)
                {
                    Point pos = light.Position;
                    if (pos.x >= 0 && pos.x < 1000 &&
                        pos.y >= 0 && pos.y < 1000)
                    {
                        characters[pos.x, pos.y] = '#';
                    }
                }

                PrintChars(characters);
                await Task.Delay(500);
            }
        }

        private static void PrintChars(char[,] characters)
        {
            Console.Clear();

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < characters.GetLength(0); i++)
            {
                char[] row = new char[characters.GetLength(1)];
                for (int j = 0; j < row.Length; j++)
                    row[j] = characters[i, j];

                string line = new string(row);
                sb.AppendLine(line);
            }

            Console.Write(sb.ToString());

        }
    }

    /// <summary>
    /// Describes an elvish light.
    /// </summary>
    internal class Light
    {
        public Light(Point position, Point velocity)
        {
            Position = position;
            Velocity = velocity;
        }

        #region Properties

        public Point Position { get; private set; }
        public Point Velocity { get; private set; }

        #endregion Properties

        public override string ToString()
        {
            return $"pos: ({Position}), vel: ({Velocity})";
        }
    }

    internal struct Point
    {
        public int x, y;

        public override string ToString()
        {
            return $"{x}, {y}";
        }
    }
}