using AOC.Resources;
using System;
using System.IO;
using System.Text;

namespace day_10a
{
    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] input = General.GetInputFromRelativePath("day10.txt").Result;
            Light[] lights = ParseInput(input);

            MoveLights(lights);
            PrintLights(lights);

            Console.WriteLine("The result has been printed to \"result.txt\".");
            Console.ReadKey();
        }

        /// <summary>
        /// Parses the input into an array of lights.
        /// </summary>
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

        /// <summary>
        /// Moves the lights until they forms a message.
        /// </summary>
        private static void MoveLights(Light[] lights)
        {
            const int frameSize = 10000;
            const int offset = frameSize / 2;
            bool?[,] visualLights = new bool?[frameSize, frameSize];

            int counter = 0;
            const int maxCounterVal = 1000;

            // Glancing at the data, it's clear that the first 10000 iterations don't need checking.
            // Adjust as needed
            foreach (Light light in lights)
                light.Step(10000);

            while (true)
            {
                // Move all lights
                foreach (Light light in lights)
                    light.Step();

                Console.Write(".");
                if (counter % 20 == 0)
                    Console.WriteLine("Performed itteration {0}", counter);

                if (counter >= maxCounterVal)
                {
                    Console.WriteLine("No answer found");
                    return;
                }

                counter++;

                // Write visual
                int neightborLights = 0;
                foreach (Light light in lights)
                {
                    Point pos = light.Position + offset;

                    if (pos.x < 0 || pos.x >= frameSize ||
                        pos.y < 0 || pos.y >= frameSize)
                    {
                        goto NextLoop;
                    }

                    for (int i = -1; i <= 1; i++)
                        for (int j = -1; j <= 1; j++)
                        {
                            int neighborX = pos.x + i;
                            int neighborY = pos.y + j;

                            if (neighborX < 0 || neighborX >= frameSize || neighborY < 0 || neighborY >= frameSize)
                                continue;

                            if (visualLights[neighborX, neighborY] != null)
                            {
                                visualLights[neighborX, neighborY] = true;
                                visualLights[pos.x, pos.y] = true;

                                neightborLights++;
                                goto NextLight;
                            }
                        }

                    visualLights[pos.x, pos.y] = false;

                NextLight:;
                }

                // Check if all lights are touching at least a single lights.
                foreach (Light light in lights)
                {
                    Point pos = light.Position + offset;
                    if (visualLights[pos.x, pos.y] == false)
                        goto NextLoop;
                }

                return;

            NextLoop:;

                // Clear visual
                visualLights = new bool?[frameSize, frameSize];

                if (neightborLights > 300)
                {
                    Console.WriteLine("Found one: {0} neighbors!", neightborLights);
                    return;
                }
            }
        }

        /// <summary>
        /// Prints the lights to a file.
        /// </summary>
        private static void PrintLights(Light[] lights)
        {
            const int frameSize = 500;
            const int offset = frameSize / 2;

            // Create image
            char[,] characters = new char[frameSize, frameSize];
            for (int i = 0; i < frameSize; i++)
                for (int j = 0; j < frameSize; j++)
                    characters[i, j] = '.';

            int letterCount = 0;
            foreach (Light light in lights)
            {
                Point pos = light.Position + offset;
                if (pos.x >= 0 && pos.x < frameSize &&
                    pos.y >= 0 && pos.y < frameSize)
                {
                    characters[pos.y, pos.x] = '#';
                    letterCount++;
                }
            }

            Console.WriteLine("There were {0} letters, out of {1} lights", letterCount, lights.Length);

            // Write to file.
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < characters.GetLength(0); i++)
            {
                char[] row = new char[characters.GetLength(1)];
                for (int j = 0; j < row.Length; j++)
                    row[j] = characters[i, j];

                string line = new string(row);
                sb.AppendLine(line);
            }
            File.WriteAllText(@".\result.txt", sb.ToString());
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

        #region Methods

        /// <summary>
        /// Performs a single step by adding velocity to position.
        /// </summary>
        public void Step()
        {
            Position += Velocity;
        }

        /// <summary>
        /// Perform a given number of steps.
        /// </summary>
        public void Step(int steps)
        {
            Position += Velocity * steps;
        }

        #endregion Methods

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

        public static Point operator +(Point p1, Point p2)
        {
            p1.x += p2.x;
            p1.y += p2.y;

            return p1;
        }

        public static Point operator +(Point point, int addition)
        {
            point.x += addition;
            point.y += addition;

            return point;
        }

        public static Point operator *(Point point, int multiplier)
        {
            point.x *= multiplier;
            point.y *= multiplier;

            return point;
        }
    }
}