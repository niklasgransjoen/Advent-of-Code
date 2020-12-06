using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day05
{
    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var boardingPasses = ParseInput(input);
            var freeSeats = GetFreeSeats(boardingPasses);
            var result = CalculateResult(boardingPasses, freeSeats);

            AOCUtils.PrintResult("The ID of the seat is", result);
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

        private static IEnumerable<BoardingPass> GetFreeSeats(BoardingPass[] boardingPasses)
        {
            const int rows = 128;
            const int columns = 8;

            var map = boardingPasses.ToHashSet();
            for (int i = 0; i < rows; i++)
            {
                for (int j = 0; j < columns; j++)
                {
                    var seat = new BoardingPass(i, j);
                    if (!map.Contains(seat))
                    {
                        yield return seat;
                    }
                }
            }
        }

        private static IEnumerable<int> CalculateIDs(IEnumerable<BoardingPass> boardingPasses) => boardingPasses.Select(p => p.Row * 8 + p.Column);

        private static int CalculateResult(IEnumerable<BoardingPass> boardingPasses, IEnumerable<BoardingPass> freeSeats)
        {
            var takenIDs = CalculateIDs(boardingPasses).ToHashSet();
            return CalculateIDs(freeSeats)
                .Where(id => takenIDs.Contains(id - 1) && takenIDs.Contains(id + 1))
                .Single();
        }

        private readonly struct BoardingPass : IEquatable<BoardingPass>
        {
            public BoardingPass(int row, int column)
            {
                Row = row;
                Column = column;
            }

            public int Row { get; }
            public int Column { get; }

            public override bool Equals(object? obj) => obj is BoardingPass other && Equals(other);

            public bool Equals(BoardingPass other)
            {
                return
                    Row == other.Row &&
                    Column == other.Column;
            }

            public override int GetHashCode() => HashCode.Combine(Row, Column);
        }
    }
}