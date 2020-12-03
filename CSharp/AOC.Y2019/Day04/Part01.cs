using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;

namespace AOC.Y2019.Day04
{
    /**
     * https://adventofcode.com/2019/day/4
     */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            string input = context.Input;
            ParseInput(input, out int lowerLimit, out int upperLimit);

            int[] passwords = FindValidPasswords(lowerLimit, upperLimit);
            int result = passwords.Length;

            AOCUtils.PrintResult(result);
        }

        private static void ParseInput(string input, out int lowerLimit, out int upperlimit)
        {
            ReadOnlySpan<char> inputSpan = input.AsSpan();
            int delimiter = inputSpan.IndexOf('-');

            ReadOnlySpan<char> lowerLimitStr = inputSpan.Slice(0, delimiter);
            ReadOnlySpan<char> upperLimitStr = inputSpan.Slice(delimiter + 1, inputSpan.Length - delimiter - 1);

            lowerLimit = int.Parse(lowerLimitStr);
            upperlimit = int.Parse(upperLimitStr);
        }

        private static int[] FindValidPasswords(int lowerLimit, int upperLimit)
        {
            // Init with large capacity.
            HashSet<int> result = new HashSet<int>(10000);

            for (int i = lowerLimit; i <= upperLimit; i++)
            {
                string value = i.ToString(CultureInfo.InvariantCulture);
                int steps = ValidPassword(value);

                if (steps == 0)
                {
                    result.Add(i);
                }
                else
                {
                    // Skip until the next potential password.
                    i += steps - 1;
                }
            }

            return result.ToArray();
        }

        /// <summary>
        /// Validate password. Returns 0 if current value is valid, otherwise the number of steps until the next potentially valid number can be found.
        /// </summary>
        private static int ValidPassword(string value)
        {
            bool hasNeighboringDigits = false;
            for (int i = 0; i < value.Length - 1; i++)
            {
                char val1 = value[i];
                char val2 = value[i + 1];

                if (val1 > val2)
                {
                    // Find number of steps until next value.
                    // That's not until all following values are *equal* to val1.

                    int digits = value.Length - i;
                    string validSuffixStr = new string(val1, digits);
                    int validSuffix = int.Parse(validSuffixStr, CultureInfo.InvariantCulture);

                    ReadOnlySpan<char> currentSuffixSpan = value.AsSpan().Slice(i, value.Length - i);
                    int currentSuffix = int.Parse(currentSuffixSpan);

                    return validSuffix - currentSuffix;
                }

                // Check if the neighboring digits are the same.
                hasNeighboringDigits |= val1 == val2;
            }

            return hasNeighboringDigits ? 0 : 1;
        }
    }
}