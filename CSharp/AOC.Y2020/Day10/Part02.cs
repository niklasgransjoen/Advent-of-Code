using System.Linq;

namespace AOC.Y2020.Day10
{
    /**
      * https://adventofcode.com/2020/day/10
      */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetIntegerInput();
            var diffs = CreateAdapterDiffs(input);
            var combinations = CountCombinations(diffs);

            AOCUtils.PrintResult(combinations);
        }

        private static int[] CreateAdapterDiffs(int[] input)
        {
            var orderedInput = input.OrderBy(val => val).ToArray();

            var initialValue = 0;
            var internalAdapterVal = orderedInput[^1] + 3;

            var bigSeries = orderedInput.Append(internalAdapterVal);
            var smallSeries = orderedInput.Prepend(initialValue);

            return bigSeries.Zip(smallSeries, (big, small) => big - small).ToArray();
        }

        private static long CountCombinations(int[] diffs)
        {
            /*
             * Occurrences of:
             * - 3s are immutable.
             * - single 1s are immutable.
             * - double 1s give two combinations.
             * - tripple 1s give four combinations
             * - any extra 1 after this gives an extra of three combinations
             */

            long totalCombinations = 1;
            for (int i = 0; i < diffs.Length; i++)
            {
                if (diffs[i] == 3)
                    continue;

                int onesCount = 1;
                while (diffs[i + 1] == 1)
                {
                    onesCount++;
                    i++;
                }

                checked
                {
                    totalCombinations *= onesCount switch
                    {
                        1 or 2 => onesCount,
                        _ => 4 + 3 * (onesCount - 3),
                    };
                }
            }

            return totalCombinations;
        }
    }
}