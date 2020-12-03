using System;
using System.Diagnostics;

namespace AOC.Y2018.Day11
{
    /*
     * https://adventofcode.com/2018/day/11
     */

    /// <summary>
    /// Calculation with this took 4 minutes and 58 seconds.
    /// Check out the alternative day11b solution at day_11b2.
    /// </summary>
    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string input = context.Input;
            int gridSerialNumber = int.Parse(input);

            int[,] grid = CreateGrid(gridSerialNumber);
            int[][,] sums = SumGridCells(grid);

            GetMaxCellCoordinates(sums, out int x, out int y, out int squareSize);

            Console.WriteLine("The result is \"{0},{1},{2}\"", x, y, squareSize);
            Console.WriteLine("Calculation tok {0} ms", sw.ElapsedMilliseconds);
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

            Console.WriteLine("Constructed power grid");
            return grid;
        }

        /// <summary>
        /// Returns a 2D array containing the sum of all 3x3 squares.
        /// </summary>
        /// <remarks>The sum of a 3x3 square has the same coordinates as the top-left corner of the original region.</remarks>
        private static int[][,] SumGridCells(int[,] grid)
        {
            // Allocate memory.
            int[][,] sums = new int[300][,];
            for (int i = 0; i < 300; i++)
                sums[i] = new int[300 - i, 300 - i];

            for (int i = 0; i < 300; i++)
            {
                int sumArraySize = 300 - i;
                for (int x = 0; x < sumArraySize; x++)
                    for (int y = 0; y < sumArraySize; y++)
                    {
                        sums[i][x, y] = AddGridSquare(grid, x, y, i + 1);
                    }

                Console.WriteLine("Calculated for squares of size {0}x{0}", i + 1);
            }

            return sums;
        }

        /// <summary>
        /// Add together the square with the given size and given coordinates as its top-left corner.
        /// </summary>
        private static int AddGridSquare(int[,] grid, int x, int y, int squareSize)
        {
            int gridSum = 0;
            for (int x2 = x; x2 < x + squareSize; x2++)
                for (int y2 = y; y2 < y + squareSize; y2++)
                {
                    gridSum += grid[x2, y2];
                }

            return gridSum;
        }

        /// <summary>
        /// Calculates the coordinates of the cell with the highest value.
        /// </summary>
        private static void GetMaxCellCoordinates(int[][,] sums, out int x, out int y, out int squareSize)
        {
            int maxVal = int.MinValue;
            int xMax = 0;
            int yMax = 0;
            int squareSizeMax = 1;

            for (squareSize = 0; squareSize < 300; squareSize++)
            {
                for (x = 0; x < sums[squareSize].GetLength(0); x++)
                    for (y = 0; y < sums[squareSize].GetLength(1); y++)
                    {
                        if (maxVal < sums[squareSize][x, y])
                        {
                            maxVal = sums[squareSize][x, y];
                            xMax = x;
                            yMax = y;
                            squareSizeMax = squareSize;
                        }
                    }
            }

            x = xMax;
            y = yMax;
            squareSize = squareSizeMax + 1;
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