using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2020.Day10
{
    /**
      * https://adventofcode.com/2020/day/10
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetIntegerInput();
            var differences = FindDifferences(input);

            var result = differences.Ones * (differences.Threes + 1);
            AOCUtils.PrintResult(result);
        }

        private static AdapterDifferences FindDifferences(int[] input)
        {
            var diffs = new Dictionary<int, int>
            {
                { 1, 0 },
                { 2, 0 },
                { 3, 0 },
            };

            int current = 0;
            foreach (var value in input.OrderBy(val => val))
            {
                var diff = value - current;
                if (diff is < 1 or > 3)
                    throw new Exception($"A difference of '{diff}' is invalid.");

                diffs[diff]++;

                current = value;
            }

            return new AdapterDifferences(diffs[1], diffs[2], diffs[3]);
        }

        private sealed record AdapterDifferences(int Ones, int Twos, int Threes);
    }
}