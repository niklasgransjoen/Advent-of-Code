using AOC.Resources;
using System;

namespace day11a
{
    /*
     * https://adventofcode.com/2018/day/11
     */

    internal static class Program
    {
        private static void Main()
        {
            string input = General.ReadSingleLineInput(Days.Day11);
            int gridSerialNumber = int.Parse(input);

            int[,] grid = CreateGrid(gridSerialNumber);
            int[,] sums = SumGridCells(grid);

            GetMaxCellCoordinates(sums, out int x, out int y);

            Console.WriteLine("The coordinates of the top-left fuel cell of the 3x3 square with the largest total power is {0}, {1}.", x, y);
            Console.ReadKey();
        }

        /// <summary>
        /// Constructs the grid, calculates power levels for each cell.
        /// </summary>
        private static int[,] CreateGrid(int gridSerialNumber)
        {
            int[,] grid = new int[300, 300];

            for (int x = 0; x < 300; x++)
                for (int y = 0; y < 300; y++)
                    grid[x, y] = CalculatePowerLevel(x, y, gridSerialNumber);

            return grid;
        }

        /// <summary>
        /// Returns a 2D array containing the sum of all 3x3 squares.
        /// </summary>
        /// <remarks>The sum of a 3x3 square has the same coordinates as the top-left corner of the original region.</remarks>
        private static int[,] SumGridCells(int[,] grid)
        {
            int[,] sums = new int[grid.GetLength(0) - 2, grid.GetLength(1) - 2];

            for (int x = 0; x < sums.GetLength(0); x++)
                for (int y = 0; y < sums.GetLength(1); y++)
                {
                    sums[x, y] = AddGridSquare(grid, x, y);
                }

            return sums;
        }

        /// <summary>
        /// Add together the 3x3 square with the given coordinates as its top-left corner.
        /// </summary>
        private static int AddGridSquare(int[,] grid, int x, int y)
        {
            int gridSum = 0;
            for (int x2 = x; x2 < x + 3; x2++)
                for (int y2 = y; y2 < y + 3; y2++)
                {
                    gridSum += grid[x2, y2];
                }

            return gridSum;
        }

        /// <summary>
        /// Calculates the coordinates of the cell with the highest value.
        /// </summary>
        private static void GetMaxCellCoordinates(int[,] grid, out int x, out int y)
        {
            int maxVal = int.MinValue;
            int xMax = 0;
            int yMax = 0;

            for (x = 0; x < grid.GetLength(0); x++)
                for (y = 0; y < grid.GetLength(1); y++)
                {
                    if (maxVal < grid[x, y])
                    {
                        maxVal = grid[x, y];
                        xMax = x;
                        yMax = y;
                    }
                }

            x = xMax;
            y = yMax;
        }

        /// <summary>
        /// Calculates the power level for a single cell.
        /// </summary>
        private static int CalculatePowerLevel(int x, int y, int gridSerialNumber)
        {
            int rackId = x + 10;
            int powerLevel = rackId * y;
            powerLevel += gridSerialNumber;
            powerLevel *= rackId;

            powerLevel = (powerLevel / 100) - (powerLevel / 1000) * 10;

            return powerLevel - 5;
        }
    }
}