using System;

namespace AOC.Y2020.Day09
{
    /**
      * https://adventofcode.com/2020/day/9
      */

    public static class Part01
    {
        private const int PreambleSize = 25;

        public static void Exec(AOCContext context)
        {
            var input = context.GetBigIntegerInput();
            var result = FindFirstInvalidNumber(input);

            AOCUtils.PrintResult("The first invalid number is", result);
        }

        private static long FindFirstInvalidNumber(long[] input)
        {
            for (int i = PreambleSize; i < input.Length; i++)
            {
                var preamble = input.AsSpan(i - PreambleSize, PreambleSize);
                var number = input[i];

                if (!IsSumOfDistinctNumbers(number, preamble))
                    return number;
            }

            throw new Exception("No invalid number could be found.");
        }

        private static bool IsSumOfDistinctNumbers(long number, Span<long> preamble)
        {
            for (int i = 0; i < preamble.Length - 1; i++)
            {
                for (int j = i + 1; j < preamble.Length; j++)
                {
                    var num1 = preamble[i];
                    var num2 = preamble[j];
                    if (num1 != num2 && num1 + num2 == number)
                        return true;
                }
            }

            return false;
        }
    }
}