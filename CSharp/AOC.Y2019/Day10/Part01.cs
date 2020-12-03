using System;

namespace AOC.Y2019.Day10
{
    /**
     * https://adventofcode.com/2019/day/10
     */

    public static class Part01
    {
        private const char Asteroid = '#';

        public static void Exec(AOCContext context)
        {
            string[] input = context.GetInputLines();
            int result = CalculateResult(input);

            AOCUtils.PrintResult(result);
        }

        private static int CalculateResult(string[] input)
        {
            // Convert input to usable format.
            char[][] convertedInput = new char[input.Length][];
            for (int i = 0; i < input.Length; i++)
            {
                convertedInput[i] = new char[input[i].Length];
                for (int j = 0; j < input[i].Length; j++)
                {
                    convertedInput[i][j] = input[i][j];
                }
            }

            // Create array to copy input to, so that it can be changed in the process.
            char[][] inputCopy = new char[input.Length][];
            for (int i = 0; i < input.Length; i++) inputCopy[i] = new char[input[i].Length];

            int result = 0;
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] != Asteroid)
                        continue;

                    // Copy input
                    for (int i = 0; i < input.Length; i++)
                        for (int j = 0; j < input[i].Length; j++)
                            inputCopy[i][j] = convertedInput[i][j];

                    int calcResult = CalculateSightings(inputCopy, x, y);
                    if (calcResult > result)
                        result = calcResult;
                }
            }

            return result;
        }

        private static int CalculateSightings(char[][] input, int x1, int y1)
        {
            // Remove current asteroid.
            input[y1][x1] = '.';

            int result = 0;
            for (int y2 = 0; y2 < input.Length; y2++)
            {
                for (int x2 = 0; x2 < input[y2].Length; x2++)
                {
                    if (input[y2][x2] != Asteroid)
                        continue;

                    // Looking at asteroid.
                    result++;

                    // Remove all asteroids in same line of sight relative to current asteroid.
                    RemoveAsteroids(input, x1, y1, x2, y2);
                }
            }

            return result;
        }

        private static void RemoveAsteroids(char[][] input, int x1, int y1, int x2, int y2)
        {
            int x = x2 - x1;
            int y = y2 - y1;

            int xSign = Math.Sign(x);
            int ySign = Math.Sign(y);

            x = Math.Abs(x);
            y = Math.Abs(y);

            Simplify(ref x, ref y);

            x *= xSign;
            y *= ySign;

            int yDelta = y1 + y;
            int xDelta = x1 + x;
            do
            {
                input[yDelta][xDelta] = '.';

                yDelta += y;
                xDelta += x;

                if (yDelta < 0 || yDelta >= input.Length) break;
                if (xDelta < 0 || xDelta >= input[yDelta].Length) break;
            } while (true);
        }

        private static void Simplify(ref int numerator, ref int denominator)
        {
            if (denominator == 0)
            {
                numerator = 1;
                return;
            }

            if (numerator == 0)
            {
                denominator = 1;
                return;
            }

            // Always keep a positive denominator
            if (denominator < 0)
            {
                numerator *= -1;
                denominator *= -1;
            }

            int gcd = findGCD(numerator, denominator);
            numerator /= gcd;
            denominator /= gcd;

            static int findGCD(int iNo1, int iNo2)
            {
                iNo1 = Math.Abs(iNo1);
                iNo2 = Math.Abs(iNo2);

                do
                {
                    if (iNo1 < iNo2)
                    {
                        int tmp = iNo1;
                        iNo1 = iNo2;
                        iNo2 = tmp;
                    }

                    iNo1 %= iNo2;
                } while (iNo1 != 0);

                return iNo2;
            }
        }
    }
}