using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2015.Day05
{
    public static class Part01
    {
        private static readonly HashSet<char> _vowels = new HashSet<char> { 'a', 'e', 'i', 'o', 'u' };

        public static void Exec(AOCContext context)
        {
            string[] input = context.GetInputLines();
            string[] niceStrings = FindNiceStrings(input);

            int result = niceStrings.Length;
            AOCUtils.PrintResult(result);
        }

        private static string[] FindNiceStrings(string[] input)
        {
            List<string> niceStrings = new List<string>();
            foreach (var line in input)
            {
                if (!HasIllegalLetters(line) &&
                    FulfillsVowelRequirement(line) &&
                    HasDoubleLetter(line))
                {
                    niceStrings.Add(line);
                }
            }

            return niceStrings.ToArray();
        }

        private static bool HasIllegalLetters(string input)
        {
            return input.Contains("ab") ||
                   input.Contains("cd") ||
                   input.Contains("pq") ||
                   input.Contains("xy");
        }

        private static bool FulfillsVowelRequirement(string input)
        {
            return input.Count(c => _vowels.Contains(c)) >= 3;
        }

        private static bool HasDoubleLetter(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                if (input[i] == input[i + 1])
                    return true;
            }

            return false;
        }
    }
}