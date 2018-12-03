using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace day_1b
{
    /* 
     * You notice that the device repeats the same frequency change list over and over.
     * To calibrate the device, you need to find the first frequency it reaches twice.
     * 
     * https://adventofcode.com/2018/day/1
     */

    internal class Program
    {
        private static void Main(string[] args)
        {
            string[] input = General.GetInput();
            int[] parsedInput = ParseInput(input);

            int result = Task.Run(() => CalculateResult(parsedInput)).Result;

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

        /// <summary>
        /// Finds the first frequency that's repeated.
        /// <para>Loops until such a frequency is found.</para>
        /// </summary>
        private static int CalculateResult(int[] input)
        {
            List<int> previousValues = new List<int>();
            int result = 0;

            while (true)
            {
                foreach (int value in input)
                {
                    result += value;

                    if (previousValues.Contains(result))
                        return result;

                    previousValues.Add(result);
                }
            }
        }
    }
}