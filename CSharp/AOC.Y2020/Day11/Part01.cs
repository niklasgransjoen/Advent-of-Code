using System;
using System.Linq;

namespace AOC.Y2020.Day11
{
    /**
      * https://adventofcode.com/2020/day/11
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var initialLayout = ParseLayout(input);
            SimulateUntilStable(initialLayout);

            var takenSeats = initialLayout.SelectMany(cells => cells).Count(state => state == CellState.TakenSeat);
            AOCUtils.PrintResult("The number of occupied seats are", takenSeats);
        }

        private static CellState[][] ParseLayout(string[] input)
        {
            if (input.Length == 0)
                throw new Exception("Input cannot be empty.");

            var height = input.Length;
            var width = input[0].Length;
            var layout = ArrayUtility.Create2D<CellState>(width, height);

            for (int x = 0; x < width; x++)
            {
                for (int y = 0; y < height; y++)
                {
                    if (input[y].Length <= x)
                        throw new Exception("Input is malformed.");

                    var c = input[y][x];
                    layout[y][x] = c switch
                    {
                        '.' => CellState.Floor,
                        'L' => CellState.FreeSeat,
                        _ => throw new Exception($"Unexpected char '{c}'."),
                    };
                }
            }

            return layout;
        }

        private static void SimulateUntilStable(CellState[][] current)
        {
            var next = ArrayUtility.Create2D<CellState>(current[0].Length, current.Length);
            CopyJaggedTo(current, next);

            while (true)
            {
                Simulate(current, next);
                if (current.SelectMany(cells => cells)
                    .SequenceEqual(next.SelectMany(cells => cells)))
                {
                    return;
                }

                CopyJaggedTo(next, current);
            }

            static void CopyJaggedTo(CellState[][] from, CellState[][] to)
            {
                for (int i = 0; i < from.Length; i++)
                {
                    from[i].CopyTo(to[i], index: 0);
                }
            }
        }

        private static void Simulate(CellState[][] current, CellState[][] next)
        {
            for (int y = 0; y < current.Length; y++)
            {
                for (int x = 0; x < current[y].Length; x++)
                {
                    var neighborCount = getNeighborCount(current, x, y);
                    if (current[y][x] is CellState.FreeSeat && neighborCount == 0)
                        next[y][x] = CellState.TakenSeat;
                    else if (current[y][x] is CellState.TakenSeat && neighborCount >= 4)
                        next[y][x] = CellState.FreeSeat;
                }
            }

            static int getNeighborCount(CellState[][] layout, int posX, int posY)
            {
                int count = 0;

                for (int y = posY - 1; y <= posY + 1; y++)
                {
                    if (y < 0 || y >= layout.Length)
                        continue;

                    for (int x = posX - 1; x <= posX + 1; x++)
                    {
                        if (x < 0 || x >= layout[y].Length)
                            continue;

                        if (x == posX && y == posY)
                            continue;

                        if (layout[y][x] == CellState.TakenSeat)
                            count++;
                    }
                }

                return count;
            }
        }

        private enum CellState
        {
            Floor,
            FreeSeat,
            TakenSeat,
        }
    }
}