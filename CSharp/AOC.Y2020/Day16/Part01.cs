using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text.RegularExpressions;

namespace AOC.Y2020.Day16
{
    /**
      * https://adventofcode.com/2020/day/16
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var rawInput = context.GetInputLines();
            var input = ParseInput(rawInput);
            var invalidFields = FindInvalidFields(input);

            var result = invalidFields.Sum();
            AOCUtils.PrintResult("Ticket scanning error rate", result);
        }

        private static PuzzleInput ParseInput(string[] input)
        {
            var ranges = new List<Range>();
            var fields = new List<int>();

            // Ranges
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                    break;

                var match = Regex.Match(line, @"^[\w ]+: (\d+)-(\d+) or (\d+).(\d+)$");
                if (!match.Success)
                    throw new Exception($"Invalid input '{line}'.");

                var min1 = int.Parse(match.Groups[1].Value, CultureInfo.InvariantCulture);
                var max1 = int.Parse(match.Groups[2].Value, CultureInfo.InvariantCulture);
                var min2 = int.Parse(match.Groups[3].Value, CultureInfo.InvariantCulture);
                var max2 = int.Parse(match.Groups[4].Value, CultureInfo.InvariantCulture);

                ranges.Add(new Range(min1, max1));
                ranges.Add(new Range(min2, max2));
            }

            // Fields
            var fieldStart = input.TakeWhile(line => line != "nearby tickets:").Count() + 1;
            foreach (var line in input.Skip(fieldStart))
            {
                var numbers = AOCUtils.StringToInt(line.Split(','));
                fields.AddRange(numbers);
            }

            return new PuzzleInput(ranges.ToArray(), fields.ToArray());
        }

        private static int[] FindInvalidFields(PuzzleInput input)
        {
            var result = new List<int>();
            foreach (var field in input.Fields)
            {
                if (!ValidateField(field, input.Ranges))
                    result.Add(field);
            }

            return result.ToArray();
        }

        private static bool ValidateField(int field, Range[] ranges)
        {
            foreach (var range in ranges)
            {
                if (field >= range.Min && field <= range.Max)
                    return true;
            }

            return false;
        }

        record Range(int Min, int Max);
        record PuzzleInput(Range[] Ranges, int[] Fields);
    }
}