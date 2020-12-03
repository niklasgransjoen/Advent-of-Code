using AOC.Resources;
using System;
using System.Linq;

namespace AOC.Y2018.Day06
{
    /*
     * https://adventofcode.com/2018/day/6
     */

    public static class Part02
    {
        private const int MaxDistance = 10000;

        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day06);
            GetCoordinates(input, out int[] x, out int[] y);
            int[,] map = CreateMap(x, y);
            int result = FindRegionSize(map);

            General.PrintResult("The size of the region is", result);
        }

        /// <summary>
        /// Extracts the coordinates from the input.
        /// </summary>
        private static void GetCoordinates(string[] input, out int[] x, out int[] y)
        {
            x = new int[input.Length];
            y = new int[input.Length];

            for (int i = 0; i < input.Length; i++)
            {
                string[] rawCoordinate = input[i].Split(',');
                x[i] = int.Parse(rawCoordinate[0]);
                y[i] = int.Parse(rawCoordinate[1].Trim());
            }
        }

        /// <summary>
        /// Generates a map from the given coordinates.
        /// The map shows the points closest to each point defined by the given x and y coordinates.
        /// <para>The numbers used to occupy a space is the index of the point closest to it plus one.</para>
        /// </summary>
        private static int[,] CreateMap(int[] xs, int[] ys)
        {
            int xMax = xs.Max() + 1;
            int yMax = ys.Max() + 1;
            int[,] map = new int[xMax, yMax];

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    int distance = 0;
                    for (int k = 0; k < xs.Length; k++)
                    {
                        distance += GetManhattenDistance(i, j, xs[k], ys[k]);
                        if (distance >= MaxDistance)
                            goto nextCoordinate;
                    }

                    map[i, j] = 1;

                    nextCoordinate:;
                }

            return map;
        }

        private static int GetManhattenDistance(int x1, int y1, int x2, int y2) => Math.Abs(x2 - x1) + Math.Abs(y2 - y1);

        private static int FindRegionSize(int[,] map)
        {
            int result = 0;
            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                    result += map[i, j];

            return result;
        }
    }
}