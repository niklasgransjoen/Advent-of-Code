using AOC.Resources;
using System;
using System.Linq;

namespace day_1a
{
    /*
     * After feeling like you've been falling for a few minutes, you look at the device's tiny screen.
     * "Error: Device must be calibrated before first use. Frequency drift detected. Cannot maintain
     * destination lock." Below the message, the device shows a sequence of changes in frequency (your puzzle
     * input). A value like +6 means the current frequency increases by 6; a value like -3 means the current
     * frequency decreases by 3.
     *
     * Starting with a frequency of zero, what is the resulting frequency after all of the changes in frequency
     * have been applied?
     *
     * https://adventofcode.com/2018/day/1
     */

    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] input = General.GetInput();
            int[] parsedInput = ParseInput(input);
            int result = parsedInput.Sum();

            Console.WriteLine("The result is: {0}", result);
            Console.ReadKey();
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
    }
}