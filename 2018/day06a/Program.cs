using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;

namespace day6a
{
    /*
     * Using only the Manhattan distance, determine the area around each coordinate by counting the number of integer
     * X, Y locations that are closest to that coordinate (and aren't tied in distance to any other coordinate).
     *
     * What is the size of the largest area that isn't infinite?
     *
     * https://adventofcode.com/2018/day/6
     */

    internal static class Program
    {
        private static void Main()
        {
            string[] input = General.ReadInput(Days.Day06);
            GetCoordinates(input, out int[] x, out int[] y);
            int[,] map = CreateMap(x, y);
            FilterMap(map);
            int result = FindLargestArea(map, input.Length);

            General.PrintResult("The largest are that isn't infinite has an area of", result);
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
            int[,] coordinateMap = new int[xMax, yMax];
            int[,] map = new int[xMax, yMax];

            // Marks coordinates with their index + 1.
            for (int i = 0; i < xs.Length; i++)
                coordinateMap[xs[i], ys[i]] = i + 1;

            // Traverse map and find closest neighbour to every point.
            for (int i = 0; i < xMax; i++)
                for (int j = 0; j < yMax; j++)
                    map[i, j] = GetNeighbour(coordinateMap, i, j);

            return map;
        }

        /// <summary>
        /// Returns the value of the closest neighbour. -1 if its a tie.
        /// </summary>
        private static int GetNeighbour(int[,] map, int X, int Y)
        {
            int distance = 0;
            while (distance < map.GetLength(0) || distance < map.GetLength(1))
            {
                int value = 0;
                for (int i = -distance; i <= distance; i++)
                    for (int j = -distance; j <= distance; j++)
                    {
                        int x = X + i;
                        int y = Y + j;
                        if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
                        {
                            int trueDistance = Math.Abs(i) + Math.Abs(j);
                            if (trueDistance != distance || map[x, y] == 0)
                                continue;

                            if (value == 0)
                                value = map[x, y];
                            else if (value != map[x, y])
                                value = -1;
                        }
                    }

                if (value != 0)
                    return value;

                distance++;
            }

            throw new Exception("No neighbours");
        }

        /// <summary>
        /// Sets the value of the given coordinate if it's not out of bounds.
        /// </summary>
        private static void SetValue(int[,] map, bool[,] mapThing, int x, int y, int value)
        {
            if (x >= 0 && x < map.GetLength(0) && y >= 0 && y < map.GetLength(1))
            {
                if (map[x, y] == 0)
                {
                    map[x, y] = value;
                    mapThing[x, y] = true;
                }
                else if (mapThing[x, y])
                    map[x, y] = -1;
            }
        }

        /// <summary>
        /// Removes "infinite" areas.
        /// </summary>
        private static void FilterMap(int[,] map)
        {
            List<int> valuesToRemove = new List<int>();
            for (int i = 0; i < map.GetLength(0); i++)
            {
                AddValue(valuesToRemove, map[i, 0]);
                AddValue(valuesToRemove, map[i, map.GetLength(1) - 1]);
            }

            for (int j = 0; j < map.GetLength(1); j++)
            {
                AddValue(valuesToRemove, map[0, j]);
                AddValue(valuesToRemove, map[map.GetLength(0) - 1, j]);
            }

            for (int i = 0; i < map.GetLength(0); i++)
                for (int j = 0; j < map.GetLength(1); j++)
                {
                    if (valuesToRemove.Contains(map[i, j]))
                        map[i, j] = -1;
                }
        }

        private static void AddValue(List<int> valuesToRemove, int value)
        {
            if (value != -1 && !valuesToRemove.Contains(value))
                valuesToRemove.Add(value);
        }

        /// <summary>
        /// Finds the largest area.
        /// </summary>
        /// <param name="pointCount">The number of points</param>
        private static int FindLargestArea(int[,] map, int pointCount)
        {
            int result = 0;
            for (int i = 1; i <= pointCount; i++)
            {
                int count = 0;
                for (int j = 0; j < map.GetLength(0); j++)
                    for (int k = 0; k < map.GetLength(1); k++)
                    {
                        if (map[j, k] == i)
                            count++;
                    }

                if (count > result)
                    result = count;
            }

            return result;
        }
    }
}