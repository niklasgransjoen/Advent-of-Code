using AOC.Resources;
using System;
using System.Diagnostics;

namespace AOC2018.Day11
{
    /*
     * https://adventofcode.com/2018/day/11
     */

    /// <summary>
    /// Calculation with this took 58 seconds.
    /// The performance increase is a result of using factors of the square dimensions.
    /// </summary>
    public static class Part02b
    {
        public static void Exec()
        {
            Stopwatch sw = new Stopwatch();
            sw.Start();

            string input = General.ReadSingleLineInput(Day.Day11);
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
                int squareSize = i + 1;

                // Find the smallest factor
                int smallestFactor = 0;
                for (int j = 2; j < squareSize; j++)
                {
                    if (squareSize % j == 0)
                    {
                        smallestFactor = j;
                        break;
                    }
                }

                int sumArraySize = 300 - i;

                // If the number is prime, calculate the sum from the original grid.
                if (smallestFactor == 0)
                {
                    for (int x = 0; x < sumArraySize; x++)
                        for (int y = 0; y < sumArraySize; y++)
                        {
                            sums[i][x, y] = AddGridSquare(grid, x, y, squareSize, increments: 1);
                        }
                }
                // If the number isn't prime, use to sum calculated for the previous factor.
                // By doing this, we're not adding up the same numbers over and over again.
                // (when calculating for 4x4 squares, 2x2 has already been calculated, so we just need the sum of those squares instead).
                else
                {
                    int largerstFactor = squareSize / smallestFactor;

                    for (int x = 0; x < sumArraySize; x++)
                        for (int y = 0; y < sumArraySize; y++)
                        {
                            sums[i][x, y] = AddGridSquare(sums[largerstFactor - 1], x, y, squareSize, increments: largerstFactor);
                        }
                }

                Console.WriteLine("Calculated for squares of size {0}x{0}", squareSize);
            }

            return sums;
        }

        /// <summary>
        /// Add together the square with the given size and given coordinates as its top-left corner.
        /// </summary>
        private static int AddGridSquare(int[,] grid, int x, int y, int squareSize, int increments)
        {
            int gridSum = 0;
            for (int x2 = x; x2 < x + squareSize; x2 += increments)
                for (int y2 = y; y2 < y + squareSize; y2 += increments)
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