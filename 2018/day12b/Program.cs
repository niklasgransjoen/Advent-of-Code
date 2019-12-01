using AOC.Resources;

namespace day12b
{
    /**
     * https://adventofcode.com/2018/day/12
     */

    /// <summary>
    /// Works with both Part One and Part Two of Day 12. Faster than my original solution.
    ///
    /// Made on the assumtion that the following rule is always true: ..... => .
    /// </summary>
    internal static class Program
    {
        /// <summary>
        /// The number of generations to simulate.
        /// </summary>
        private const ulong Generations = 50_000_000_000;

        /// <summary>
        /// How many elements to allocate memory for.
        /// </summary>
        private const int FlowerArrayAlloc = 100_000;

        private const int RuleCount = 1 << 5;

        private static void Main()
        {
            // Allocate memory
            Flower[] state = new Flower[FlowerArrayAlloc];
            Flower[] newState = new Flower[FlowerArrayAlloc];

            string[] input = General.ReadInput(Days.Day12);
            ParseInput(input, state, out bool[] rules, out int flowerCount);

            // Run all generations.
            for (ulong i = 0; i < Generations; i++)
            {
                StepGeneration(state, newState, rules, ref flowerCount);

                var tempState = state;

                state = newState;
                newState = tempState;
            }

            // Calculate result.
            int result = 0;
            for (int i = 0; i < flowerCount; i++)
            {
                if (state[i].isSet)
                    result += state[i].value;
            }

            General.PrintResult(result);
        }

        /// <summary>
        /// Parses the input into initial state and rules.
        /// </summary>
        private static void ParseInput(string[] input, Flower[] state, out bool[] rules, out int flowerCount)
        {
            // Parse state.
            string initialState = input[0].Substring(15);

            flowerCount = initialState.Length;
            for (int i = 0; i < flowerCount; i++)
            {
                if (initialState[i] == '#')
                    state[i] = new Flower(i);
            }

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
        /// <param name="flowerCount">The number of elements used in the state array.</param>
        private static void StepGeneration(Flower[] state, Flower[] newState, bool[] rules, ref int flowerCount)
        {
            bool[] flowers = new bool[5];
            for (int i = 0; i < flowerCount; i++)
            {
            }

            /*for (int i = 2; i < state.Length - 2; i++)
            {
                for (int j = 0; j < 5; j++)
                    flowers[j] = state[i + j - 2];

                int ruleIndex = flowers.ToInt();
                newState[i] = rules[ruleIndex];
            }*/
        }
    }

    /// <summary>
    /// A single flower, found on the North Pole. Wraps a single value.
    /// </summary>
    internal struct Flower
    {
        public Flower(int value)
        {
            this.value = value;
            isSet = true;
        }

        public int value;
        public bool isSet;
    }

    internal static class Extentions
    {
        /// <summary>
        /// Turns an array of bools into an int.
        /// </summary>
        /// <remarks>The LSB is the last value in the array.</remarks>
        public static int ToInt(this bool[] values)
        {
            int result = 0;
            for (int i = 0; i < values.Length; i++)
            {
                if (values[values.Length - i - 1])
                    result |= 1 << i;
            }

            return result;
        }
    }
}