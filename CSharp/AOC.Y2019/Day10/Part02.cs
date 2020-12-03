using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2019.Day10
{
    /**
     * https://adventofcode.com/2019/day/10
     */

    public static class Part02
    {
        private const char Asteroid = '#';
        private const int VaporizedPosition = 200;

        public static void Exec(AOCContext context)
        {
            string[] rawInput = context.GetInputLines();
            char[][] input = ParseInput(rawInput);

            CalculateAsteroidPosition(input, out int asteroidX, out int asteroidY);
            CalculateVaporizedPosition(input, VaporizedPosition, asteroidX, asteroidY, out int x, out int y);

            int result = x * 100 + y;
            AOCUtils.PrintResult(result);
        }

        private static char[][] ParseInput(string[] input)
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

            return convertedInput;
        }

        #region Asteroid position

        private static void CalculateAsteroidPosition(char[][] input, out int xResult, out int yResult)
        {
            // Create array to copy input to, so that it can be changed in the process.
            char[][] inputCopy = new char[input.Length][];
            for (int i = 0; i < input.Length; i++) inputCopy[i] = new char[input[i].Length];

            int result = 0;
            xResult = 0;
            yResult = 0;
            for (int y = 0; y < input.Length; y++)
            {
                for (int x = 0; x < input[y].Length; x++)
                {
                    if (input[y][x] != Asteroid)
                        continue;

                    // Copy input
                    for (int i = 0; i < input.Length; i++)
                        for (int j = 0; j < input[i].Length; j++)
                            inputCopy[i][j] = input[i][j];

                    int calcResult = CalculateSightings(inputCopy, x, y);
                    if (calcResult > result)
                    {
                        result = calcResult;
                        xResult = x;
                        yResult = y;
                    }
                }
            }
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

        #endregion Asteroid position

        #region VaporizedPosition

        private static void CalculateVaporizedPosition(char[][] input, int asteroidNumber, int x, int y, out int xResult, out int yResult)
        {
            int currentAsteroid = 0;
            HashSet<(int, int)> asteroids = new HashSet<(int, int)>();

            while (true)
            {
                // First section [0°-90°>
                for (int yDelta = 0; yDelta < y; yDelta++)
                {
                    for (int xDelta = x; xDelta < input[yDelta].Length; xDelta++)
                    {
                        if (input[yDelta][xDelta] != Asteroid)
                            continue;

                        int x1 = Math.Abs(xDelta - x);
                        int y1 = Math.Abs(yDelta - y);

                        Simplify(ref x1, ref y1);
                        asteroids.Add((x1, -y1));
                    }
                }

                var laserResult = LaserAsteroids(input, ref currentAsteroid, asteroidNumber, x, y, asteroids, sortByXDivY: true);
                if (laserResult.HasValue)
                {
                    xResult = laserResult.Value.x;
                    yResult = laserResult.Value.y;
                    return;
                }
                asteroids.Clear();

                // Second section [90°-180°>
                for (int yDelta = y; yDelta < input.Length; yDelta++)
                {
                    for (int xDelta = x + 1; xDelta < input[yDelta].Length; xDelta++)
                    {
                        if (input[yDelta][xDelta] != Asteroid)
                            continue;

                        int x1 = Math.Abs(xDelta - x);
                        int y1 = Math.Abs(yDelta - y);

                        Simplify(ref x1, ref y1);
                        asteroids.Add((x1, y1));
                    }
                }

                laserResult = LaserAsteroids(input, ref currentAsteroid, asteroidNumber, x, y, asteroids, sortByXDivY: false);
                if (laserResult.HasValue)
                {
                    xResult = laserResult.Value.x;
                    yResult = laserResult.Value.y;
                    return;
                }
                asteroids.Clear();

                // Thrid section [180°-270°>
                for (int yDelta = y + 1; yDelta < input.Length; yDelta++)
                {
                    for (int xDelta = 0; xDelta <= x; xDelta++)
                    {
                        if (input[yDelta][xDelta] != Asteroid)
                            continue;

                        int x1 = Math.Abs(xDelta - x);
                        int y1 = Math.Abs(yDelta - y);

                        Simplify(ref x1, ref y1);
                        asteroids.Add((-x1, y1));
                    }
                }

                laserResult = LaserAsteroids(input, ref currentAsteroid, asteroidNumber, x, y, asteroids, sortByXDivY: true);
                if (laserResult.HasValue)
                {
                    xResult = laserResult.Value.x;
                    yResult = laserResult.Value.y;
                    return;
                }
                asteroids.Clear();

                // Fourth section [270°-360°>
                for (int yDelta = 0; yDelta <= y; yDelta++)
                {
                    for (int xDelta = 0; xDelta < x; xDelta++)
                    {
                        if (input[yDelta][xDelta] != Asteroid)
                            continue;

                        int x1 = Math.Abs(xDelta - x);
                        int y1 = Math.Abs(yDelta - y);

                        Simplify(ref x1, ref y1);
                        asteroids.Add((-x1, -y1));
                    }
                }

                laserResult = LaserAsteroids(input, ref currentAsteroid, asteroidNumber, x, y, asteroids, sortByXDivY: false);
                if (laserResult.HasValue)
                {
                    xResult = laserResult.Value.x;
                    yResult = laserResult.Value.y;
                    return;
                }
                asteroids.Clear();
            }
        }

        private static (int x, int y)? LaserAsteroids(char[][] input, ref int currentAsteroid, int asteroidNumber, int x1, int y1, IEnumerable<(int x, int y)> rawAsteroids, bool sortByXDivY)
        {
            (int x, int y)[] asteroids = sortByXDivY ? rawAsteroids.OrderBy(arg => arg.x / (double)arg.y).ToArray() : rawAsteroids.OrderBy(arg => arg.y / (double)arg.x).ToArray();
            foreach (var asteroid in asteroids)
            {
                int xDelta = x1 + asteroid.x;
                int yDelta = y1 + asteroid.y;

                do
                {
                    if (input[yDelta][xDelta] == Asteroid)
                    {
                        input[yDelta][xDelta] = '.';
                        break;
                    }

                    xDelta += asteroid.x;
                    yDelta += asteroid.y;
                }
                while (true);

                if (currentAsteroid == asteroidNumber - 1)
                    return (xDelta, yDelta);

                currentAsteroid++;
            }

            return null;
        }

        #endregion VaporizedPosition

        #region Utilities

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

        #endregion Utilities
    }
}