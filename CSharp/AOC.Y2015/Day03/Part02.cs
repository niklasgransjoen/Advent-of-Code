using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2015.Day03
{
    /**
     * https://adventofcode.com/2015/day/3
     */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            string input = context.Input;
            HashSet<Cell> visitedHouses = CalculateVisitPattern(input);

            int result = visitedHouses.Count;
            AOCUtils.PrintResult(result);
        }

        private static HashSet<Cell> CalculateVisitPattern(string input)
        {
            HashSet<Cell> houses = new HashSet<Cell>();

            CalculateMove(input, houses, isRoboSanta: false);
            CalculateMove(input, houses, isRoboSanta: true);

            return houses;
        }

        private static void CalculateMove(string input, HashSet<Cell> houses, bool isRoboSanta)
        {
            int x = 0;
            int y = 0;

            var filteredInput = input.Where((_, i) =>
            {
                return i % 2 == (isRoboSanta ? 1 : 0);
            });

            foreach (var c in filteredInput)
            {
                houses.Add(new Cell(x, y));

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
            houses.Add(new Cell(x, y));
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