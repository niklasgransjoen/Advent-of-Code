using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day05
{
    /**
      * https://adventofcode.com/2020/day/5
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var boardingPasses = ParseInput(input);

            var maxID = boardingPasses.Select(p => p.Row * 8 + p.Column).Max();
            AOCUtils.PrintResult("The highest seat id is", maxID);
        }

        private static BoardingPass[] ParseInput(string[] input)
        {
            var regex = new Regex(@"([FB]{7})([LR]{3})");

            var result = new BoardingPass[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                var match = regex.Match(input[i]);
                if (!match.Success)
                    throw new Exception($"Invalid input '{input[i]}'.");

                var rowData = match.Groups[1].Value;
                var colData = match.Groups[2].Value;

                var row = parseRow(rowData);
                var col = parseCol(colData);

                result[i] = new BoardingPass(row, col);
            }

            return result;

            static int parseRow(string rowData)
            {
                var result = 0;
                for (int i = 0; i < rowData.Length; i++)
                {
                    if (rowData[i] == 'B')
                        result |= 1 << (rowData.Length - i - 1);
                }

                return result;
            }

            static int parseCol(string colData)
            {
                var result = 0;
                for (int i = 0; i < colData.Length; i++)
                {
                    if (colData[i] == 'R')
                        result |= 1 << (colData.Length - i - 1);
                }

                return result;
            }
        }

        private readonly struct BoardingPass
        {
            public BoardingPass(int row, int column)
            {
                Row = row;
                Column = column;
            }

            public int Row { get; }
            public int Column { get; }
        }
    }
}