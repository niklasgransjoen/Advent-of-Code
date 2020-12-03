using System;

namespace AOC.Y2020.Day01
{
    /**
     * https://adventofcode.com/2020/day/1
     */

    public static class Part02
    {
        private const int Checksum = 2020;

        public static void Exec(AOCContext context)
        {
            var input = context.GetIntegerInput();
            var result = GetResult(input, itemCount: 3);
            if (result is -1)
            {
                AOCUtils.PrintError("No result could be found.");
                return;
            }

            AOCUtils.PrintResult(result);
        }

        /// <summary>
        /// General solution. Can solve for any number of loops.
        /// </summary>
        private static int GetResult(Span<int> input, int itemCount, int sum = 0, int product = 1)
        {
            for (int i = 0; i < input.Length - itemCount + 1; i++)
            {
                var item = input[i];
                if (itemCount > 1)
                {
                    var nextInput = input[(i + 1)..];
                    var res = GetResult(nextInput, itemCount - 1, sum + item, product * item);
                    if (res != -1)
                        return res;
                }
                else if (sum + item == Checksum)
                    return product * item;
            }

            return -1;
        }
    }
}