using AOC.Resources;
using System;
using System.IO;
using System.Text;

namespace AOC.Y2018.Day10
{
    /*
     * https://adventofcode.com/2018/day/10
     */

    /// <summary>
    /// This answer is based upon the asumption that the letters are written out in such a matter that all lights neighbor minimum a single other light,
    /// and that such an alignment only occurs once.
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// We make the assumtion that the answer will fit on a 500x500 view.
        /// </summary>
        private const int ImageSize = 500;

        /// <summary>
        /// The number of steps to ignore. To speed up calculation.
        /// </summary>
        private const int StepsToSkip = 10000;

        /// <summary>
        /// Upper limit to number of steps to check.
        /// </summary>
        private const int StepLimit = 10000;

        /// <summary>
        /// Path of the file to print result to.
        /// </summary>
        private const string resultFile = @".\result.txt";

        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day10);
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
            // Glancing at the data, it's clear some iterations can be skipped.
            // This isn't necessary, and the code will work without it.
            foreach (Light light in lights)
                light.Step(StepsToSkip);

            int stepCounter = 0;

            while (true)
            {
                // Move all lights a single step.
                foreach (Light light in lights)
                    light.Step();

                // Status
                stepCounter++;
                Console.Write(".");
                if (stepCounter % 20 == 0)
                    Console.WriteLine("Performed step {0}", stepCounter);

                // Limit
                if (stepCounter > StepLimit)
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("No answer found");
                    return;
                }

                bool result = CreateAndVerifyVisual(lights);
                if (result)
                {
                    Console.WriteLine("\n");
                    Console.WriteLine("The result took {0} seconds to appear.", StepsToSkip + stepCounter);
                    return;
                }
            }
        }

        /// <summary>
        /// Creates a 2D visual in order to verify if all lights have neighbors.
        /// </summary>
        private static bool CreateAndVerifyVisual(Light[] lights)
        {
            bool?[,] visualLights = new bool?[ImageSize, ImageSize];

            // Write visual
            foreach (Light light in lights)
            {
                Point pos = light.Position + ImageSize / 2;

                if (pos.x < 0 || pos.x >= ImageSize || pos.y < 0 || pos.y >= ImageSize)
                    return false;

                visualLights[pos.x, pos.y] = false;

                for (int i = -1; i <= 1; i++)
                    for (int j = -1; j <= 1; j++)
                    {
                        if (i == j && i == 0)
                            continue;

                        int neighborX = pos.x + i;
                        int neighborY = pos.y + j;

                        if (neighborX < 0 || neighborX >= ImageSize || neighborY < 0 || neighborY >= ImageSize)
                            continue;

                        if (visualLights[neighborX, neighborY] != null)
                        {
                            visualLights[neighborX, neighborY] = true;
                            visualLights[pos.x, pos.y] = true;
                        }
                    }
            }

            // If a single light is alone, ignore result.
            foreach (Light light in lights)
            {
                Point pos = light.Position + ImageSize / 2;
                if (visualLights[pos.x, pos.y] == false)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Prints the lights to a file.
        /// </summary>
        private static void PrintLights(Light[] lights)
        {
            // Create blank image
            char[,] characters = new char[ImageSize, ImageSize];
            for (int i = 0; i < ImageSize; i++)
                for (int j = 0; j < ImageSize; j++)
                    characters[i, j] = '.';

            // Draw lights
            foreach (Light light in lights)
            {
                Point pos = light.Position + ImageSize / 2;
                if (pos.x >= 0 && pos.x < ImageSize && pos.y >= 0 && pos.y < ImageSize)
                    characters[pos.x, pos.y] = '#';
            }

            // Write to file.
            int sbCapacity = (int)Math.Pow(ImageSize, 2);
            StringBuilder sb = new StringBuilder(sbCapacity);

            for (int y = 0; y < ImageSize; y++)
            {
                char[] row = new char[ImageSize];
                for (int x = 0; x < row.Length; x++)
                    row[x] = characters[x, y];

                sb.AppendLine(new string(row));
            }

            File.WriteAllText(resultFile, sb.ToString());
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