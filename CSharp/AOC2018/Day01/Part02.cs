using AOC.Resources;
using System.Collections.Generic;
using System.Linq;

namespace AOC2018.Day01
{
    /*
     * You notice that the device repeats the same frequency change list over and over.
     * To calibrate the device, you need to find the first frequency it reaches twice.
     *
     * https://adventofcode.com/2018/day/1
     */

    public static class Part02
    {
        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day01);
            int[] parsedInput = ParseInput(input);

            int result = CalculateResult(parsedInput);

            General.PrintResult(result);
        }

        /// <summary>
        /// Parses input string.
        /// <para>Returns int array.</para>
        /// </summary>
        private static int[] ParseInput(string[] input)
        {
            int[] parsedInput = new int[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i];
                char lineOperator = line.FirstOrDefault();
                string lineValue = line.Substring(1);

                switch (lineOperator)
                {
                    case '+':
                        parsedInput[i] = int.Parse(lineValue);
                        break;

                    case '-':
                        parsedInput[i] = -int.Parse(lineValue);
                        break;

                    default:
                        continue;
                }
            }

            return parsedInput;
        }

        /// <summary>
        /// Finds the first frequency that's repeated.
        /// <para>Loops until such a frequency is found.</para>
        /// </summary>
        private static int CalculateResult(int[] input)
        {
            HashSet<int> previousValues = new HashSet<int>();
            int result = 0;

            while (true)
            {
                foreach (int value in input)
                {
                    result += value;

                    if (!previousValues.Add(result))
                        return result;
                }
            }
        }
    }
}