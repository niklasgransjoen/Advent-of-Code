using AOC.Resources;
using System;

namespace AOC.Y2018.Day12
{
    /**
     * https://adventofcode.com/2018/day/12
     */

    /// <summary>
    /// My solution to Part One of Day 12. Doesn't work with day 2.
    /// See day_12b for a solution that works better (for both parts).
    ///
    /// Made on the assumtion that the following rule is always true: ..... => .
    /// </summary>
    public static class Part01
    {
        /// <summary>
        /// The number of generations to simulate.
        /// </summary>
        private const int Generations = 20;

        /// <summary>
        /// The extra size on each side of the initial state.
        /// </summary>
        private const int Offset = Generations * 2;

        private const int RuleCount = 1 << 5;

        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day12);
            ParseInput(input, out bool[] state, out bool[] rules);

            // Run all generations.
            bool[] newState = new bool[state.Length];
            for (int i = 0; i < Generations; i++)
            {
                StepGeneration(state, newState, rules);

                var tempState = state;

                state = newState;
                newState = tempState;
            }

            // Calculate result.
            int result = 0;
            for (int i = 0; i < state.Length; i++)
            {
                if (state[i])
                    result += i - Offset;
            }

            General.PrintResult(result);
        }

        /// <summary>
        /// Parses the input into initial state and rules.
        /// </summary>
        private static void ParseInput(string[] input, out bool[] state, out bool[] rules)
        {
            // Parse state.
            string initialState = input[0].Substring(15);

            state = new bool[initialState.Length + 2 * Offset];
            for (int i = 0; i < initialState.Length; i++)
                state[i + Offset] = initialState[i] == '#';

            // Parse rules.
            rules = new bool[RuleCount];
            bool[] filter = new bool[5];
            for (int i = 2; i < input.Length; i++)
            {
                for (int k = 0; k < 5; k++)
                    filter[k] = input[i][k] == '#';

                int ruleIndex = filter.ToInt();
                rules[ruleIndex] = input[i][9] == '#';
            }
        }

        /// <summary>
        /// Steps one generation forward.
        /// </summary>
        /// <param name="state">The current state.</param>
        /// <param name="newState">The new state.</param>
        /// <param name="rules">The rules.</param>
        private static void StepGeneration(bool[] state, bool[] newState, bool[] rules)
        {
            if (state.Length != newState.Length)
                throw new ArgumentException("newState must be same size as state");

            bool[] flowers = new bool[5];
            for (int i = 2; i < state.Length - 2; i++)
            {
                for (int j = 0; j < 5; j++)
                    flowers[j] = state[i + j - 2];

                int ruleIndex = flowers.ToInt();
                newState[i] = rules[ruleIndex];
            }
        }
    }
}