using System;
using System.Linq;

namespace AOC.Y2020.Day09
{
    public static class Part02
    {
        private const int PreambleSize = 25;

        public static void Exec(AOCContext context)
        {
            var input = context.GetBigIntegerInput();
            var firstInvalidNumber = FindFirstInvalidNumber(input);
            var components = FindNumberComponents(firstInvalidNumber, input).ToArray();

            var result = components.Min() + components.Max();

            AOCUtils.PrintResult(result);
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

            static bool IsSumOfDistinctNumbers(long number, Span<long> preamble)
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

        private static Span<long> FindNumberComponents(long firstInvalidNumber, long[] input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                int lower = i;
                int current = lower;

                var sum = input[current];
                do
                {
                    current++;
                    sum += input[current];
                    if (sum == firstInvalidNumber)
                        return input.AsSpan(lower..(current + 1));
                } while (sum < firstInvalidNumber && current < input.Length - 1);
            }

            throw new Exception($"No sequence of components could be found for the number '{firstInvalidNumber}'.");
        }
    }
}