﻿namespace AOC.Y2018.Day05
{
    /*
     * The polymer is formed by smaller units which, when triggered, react with each other such that two adjacent
     * units of the same type and opposite polarity are destroyed. Units' types are represented by letters; units'
     * polarity is represented by capitalization. For instance, r and R are units with the same type but opposite
     * polarity, whereas r and s are entirely different types and do not react.
     *
     * How many units reExec after fully reacting the polymer you scanned?
     *
     * https://adventofcode.com/2018/day/5
     */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            string[] input = context.GetInputLines();
            string output = ReactInput(input[0]);

            int result = output.Length;

            AOCUtils.PrintResult("Number of units reExecing are", result);
        }

        /// <summary>
        /// Reacts the given input until it is its smallest possible size.
        /// </summary>
        private static string ReactInput(string input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                char char1 = input[i];
                char char2 = input[i + 1];

                if (char.ToLower(char1) == char.ToLower(char2) && char1 != char2)
                {
                    input = input.Remove(i, 2);

                    // Go one character back (2 back, one forward),
                    // to check if the newly removed characters results in a new reaction.
                    i -= 2;
                    if (i < -1)
                        i = -1;
                }
            }

            return input;
        }
    }
}