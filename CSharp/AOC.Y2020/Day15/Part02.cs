using System;
using System.Linq;

namespace AOC.Y2020.Day15
{
    /**
      * https://adventofcode.com/2020/day/15
      */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            var rawInput = context.GetCSVInput();
            var input = AOCUtils.StringToInt(rawInput);
            var result = FindNthNumber(input, 30_000_000);

            AOCUtils.PrintResult(result);
        }

        private static int FindNthNumber(int[] initialPosition, int n)
        {
            // We assume each initial number is unique.
            var ageMap = initialPosition.Take(initialPosition.Length - 1)
                                        .Select((number, index) => (number, index: index + 1))
                                        .ToDictionary(pair => pair.number, pair => pair.index);

            var currentIndex = ageMap.Count + 1;
            var lastSeenNumber = initialPosition[^1];

            while (currentIndex < n)
            {
                int nextNumber;
                if (ageMap.TryGetValue(lastSeenNumber, out var lastIndex))
                    nextNumber = currentIndex - lastIndex;
                else
                    nextNumber = 0;

                ageMap[lastSeenNumber] = currentIndex;
                lastSeenNumber = nextNumber;

                currentIndex++;
            }

            return lastSeenNumber;
        }
    }
}