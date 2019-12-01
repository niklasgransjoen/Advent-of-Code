using AOC.Resources;
using System;

namespace day03b
{
    /*
     * What is the ID of the only claim that doesn't overlap?
     *
     * https://adventofcode.com/2018/day/3
     */

    internal static class Program
    {
        private static void Main()
        {
            string[] input = General.ReadInput(Days.Day03);
            int result = FindNonOverlappingClaim(input);

            General.PrintResult("ID of the claim with no overlap", result);
        }

        /// <summary>
        /// Returns the id of the first non-overlapping claim found.
        /// </summary>
        private static int FindNonOverlappingClaim(string[] input)
        {
            // Parse input.
            int[] ids = new int[input.Length];
            int[,] coordinates = new int[input.Length, 2];
            int[,] dimensions = new int[input.Length, 2];

            for (int i = 0; i < input.Length; i++)
            {
                string[] rawInput = input[i].Split(' ');

                string rawId = rawInput[0].Substring(1);
                ids[i] = int.Parse(rawId);

                string[] rawCoordinates = rawInput[2].Split(new char[] { ',', ':' });
                coordinates[i, 0] = int.Parse(rawCoordinates[0]);
                coordinates[i, 1] = int.Parse(rawCoordinates[1]);

                string[] rawDimensions = rawInput[3].Split('x');
                dimensions[i, 0] = int.Parse(rawDimensions[0]);
                dimensions[i, 1] = int.Parse(rawDimensions[1]);
            }

            // Put all claims in array. Put -1 on overlaps.
            int[,] claims = new int[1000, 1000];
            for (int i = 0; i < ids.Length; i++)
            {
                int x = coordinates[i, 0];
                int y = coordinates[i, 1];
                int width = dimensions[i, 0];
                int height = dimensions[i, 1];

                for (int j = x; j < x + width; j++)
                    for (int k = y; k < y + height; k++)
                        claims[j, k] = claims[j, k] == 0 ? ids[i] : -1;
            }

            // Find the non-overlapping claim.
            for (int i = 0; i < ids.Length; i++)
                if (!HasOverlap(claims, coordinates, dimensions, i))
                    return ids[i];

            throw new Exception("There's no ID without overlap!");
        }

        /// <summary>
        /// Returns whether the claim with the given coordinates and dimensions has overlap with another claim.
        /// </summary>
        private static bool HasOverlap(int[,] claims, int[,] coordinates, int[,] dimensions, int i)
        {
            int x = coordinates[i, 0];
            int y = coordinates[i, 1];
            int width = dimensions[i, 0];
            int height = dimensions[i, 1];

            for (int j = x; j < x + width; j++)
                for (int k = y; k < y + height; k++)
                    if (claims[j, k] == -1)
                        return true;

            return false;
        }
    }
}