using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AOC.Y2020.Day13
{
    /**
      * https://adventofcode.com/2020/day/13
      */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var frequency = ParseInput(input);
            var result = CalculateResult(frequency);

            AOCUtils.PrintResult(result);
        }

        private static int?[] ParseInput(string[] input)
        {
            if (input.Length < 2)
                throw new Exception($"Input must contain at least two lines.");

            var frequency = new List<int?>();
            foreach (var item in input[1].Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (item == "x")
                    frequency.Add(null);
                else
                    frequency.Add(int.Parse(item, CultureInfo.InvariantCulture));
            }

            return frequency.ToArray();
        }

        private static ulong CalculateResult(int?[] input)
        {
            checked
            {
                // Pair the non-null factors together with their index.
                var factors = input.Select((val, index) => (val, index: (uint)index))
                    .Where(f => f.val.HasValue)
                    .Select(f => (value: (uint)f.val!, f.index))
                    .Select(f => (f.value, index: f.index % f.value))
                    .ToArray();

                // We start with the first number.
                var (firstVal, firstIndex) = factors[0];
                ulong result = firstVal - firstIndex;

                // The pattern repeats with a frequency equal to the product of all the factors (which are all primes).
                ulong period = firstVal;

                // For each following factor, we keep applying the period until it fulfills the requirement.
                // We then multiply the period with that factor.
                for (int i = 1; i < factors.Length; i++)
                {
                    var (value, index) = factors[i];
                    while ((result + index) % value != 0)
                    {
                        result += period;
                    }

                    period *= value;
                }

                return result;
            }
        }
    }
}