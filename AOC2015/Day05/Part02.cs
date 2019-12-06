using AOC.Resources;
using System.Collections.Generic;

namespace AOC2015.Day05
{
    public static class Part02
    {
        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day05);
            string[] niceStrings = FindNiceStrings(input);

            int result = niceStrings.Length;
            General.PrintResult(result);
        }

        private static string[] FindNiceStrings(string[] input)
        {
            List<string> niceStrings = new List<string>();
            foreach (var line in input)
            {
                if (HasDoubleLetterPair(line) && HasRepeatingLetter(line))
                {
                    niceStrings.Add(line);
                }
            }

            return niceStrings.ToArray();
        }

        /// <summary>
        /// Pattern xx...xx, where ... is 0 or more characters.
        /// </summary>
        private static bool HasDoubleLetterPair(string input)
        {
            for (int i = 0; i < input.Length - 3; i++)
            {
                for (int j = i + 2; j < input.Length - 1; j++)
                {
                    if (input[i] == input[j] &&
                        input[i + 1] == input[j + 1])
                    {
                        return true;
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// Pattern x_x
        /// </summary>
        private static bool HasRepeatingLetter(string input)
        {
            for (int i = 0; i < input.Length - 2; i++)
            {
                if (input[i] == input[i + 2])
                    return true;
            }

            return false;
        }
    }
}