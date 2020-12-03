using AOC.Resources;
using System.Collections.Generic;

namespace AOC.Y2015.Day03
{
    /**
     * https://adventofcode.com/2015/day/3
     */

    public static class Part01
    {
        public static void Exec()
        {
            string input = General.ReadSingleLineInput(Day.Day03);
            HashSet<Cell> visitedHouses = CalculateVisitPattern(input);

            int result = visitedHouses.Count;
            General.PrintResult(result);
        }

        private static HashSet<Cell> CalculateVisitPattern(string input)
        {
            HashSet<Cell> cells = new HashSet<Cell>();

            int x = 0;
            int y = 0;
            foreach (var c in input)
            {
                cells.Add(new Cell(x, y));

                switch (c)
                {
                    case '^':
                        y++;
                        break;

                    case 'v':
                        y--;
                        break;

                    case '>':
                        x++;
                        break;

                    case '<':
                        x--;
                        break;
                }
            }
            cells.Add(new Cell(x, y));

            return cells;
        }

        private readonly struct Cell
        {
            public Cell(int x, int y)
            {
                X = x;
                Y = y;
            }

            public int X { get; }
            public int Y { get; }
        }
    }
}