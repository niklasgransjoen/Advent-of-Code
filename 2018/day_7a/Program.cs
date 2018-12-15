using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace day_7a
{
    /*
     * https://adventofcode.com/2018/day/7
     */
    internal class Program
    {
        private static void Main(string[] args)
        {
            string result = Task.Run(async () =>
            {
                string[] input = await General.GetInputFromPath(@"..\..\..\input\day7.txt");
                //string[] input = General.GetInput();
                List<KeyValuePair<char, char>> steps = ParseInput(input);
                return CalculateResult(steps);
            }).Result;

            Console.WriteLine("The answer is: {0}", result);
            Console.ReadKey();
        }

        /// <summary>
        /// Puts all steps in a list of keyvaluepairs.
        /// </summary>
        private static List<KeyValuePair<char, char>> ParseInput(string[] input)
        {
            List<KeyValuePair<char, char>> steps = new List<KeyValuePair<char, char>>();

            foreach (string line in input)
            {
                char key = line[5];
                char value = line[36];

                steps.Add(new KeyValuePair<char, char>(key, value));
            }

            return steps;
        }

        private static string CalculateResult(List<KeyValuePair<char, char>> steps)
        {
            StringBuilder result = new StringBuilder();
            List<char> stepsList = new List<char>();

            // Finds all steps.
            foreach (KeyValuePair<char, char> pair in steps)
            {
                if (!stepsList.Contains(pair.Key))
                    stepsList.Add(pair.Key);

                if (!stepsList.Contains(pair.Value))
                    stepsList.Add(pair.Value);
            }
            stepsList.Sort();

            // Completes steps that don't depend on other steps, until all steps are completed.
            do
            {
                for (int i = 0; i < stepsList.Count; i++)
                {
                    char key = stepsList[i];
                    foreach (KeyValuePair<char, char> pair in steps)
                    {
                        char value = pair.Value;
                        if (key == value)
                            goto nextKey;
                    }

                    // Execute qualified steps in alphabetical order.
                    result.Append(key);
                    steps.RemoveAll(p => p.Key == key);
                    stepsList.Remove(key);
                    break;

                nextKey:;
                }
            }
            while (stepsList.Any());

            return result.ToString();
        }
    }
}