using AOC.Resources;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace day_5b
{
    /*
     *
     * https://adventofcode.com/2018/day/5
     */

    internal class Program
    {
        private static void Main(string[] args)
        {
            int result = Task.Run(async () =>
            {
                string[] input = await General.GetInputFromPath(@"..\..\..\input\day5.txt");
                return ReactInput(input[0]);
            }).Result;

            Console.WriteLine("Smallest number of units are: {0}", result);
            Console.ReadKey();
        }

        /// <summary>
        /// Reacts the given input until it is its smallest possible size.
        /// </summary>
        private static int ReactInput(string input)
        {
            List<char> units = new List<char>
            {
                'a', 'b', 'c', 'd', 'e', 'f', 'g', 'h', 'i', 'j', 'k', 'l', 'm',
                'n', 'o', 'p', 'q', 'r', 's', 't', 'u', 'v', 'w', 'x', 'y', 'z'
            };
            int[] result = new int[units.Count];

            for (int i = 0; i < units.Count; i++)
            {
                string inputUnitsRemoved = input
                    .Replace(units[i].ToString(), "")
                    .Replace(units[i].ToString().ToUpper(), "");

                for (int j = 0; j < inputUnitsRemoved.Length - 1; j++)
                {
                    char char1 = inputUnitsRemoved[j];
                    char char2 = inputUnitsRemoved[j + 1];

                    if (char.ToLower(char1) == char.ToLower(char2) && char1 != char2)
                    {
                        inputUnitsRemoved = inputUnitsRemoved.Remove(j, 2);

                        // Go one character back (2 back, one forward),
                        // to check if the newly removed characters results in a new reaction.
                        j -= 2;
                        if (j < -1)
                            j = -1;
                    }
                }

                result[i] = inputUnitsRemoved.Length;
            }

            return result.Min();
        }
    }
}