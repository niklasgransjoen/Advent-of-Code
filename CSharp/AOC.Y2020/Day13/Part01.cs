using System;
using System.Collections.Generic;
using System.Globalization;

namespace AOC.Y2020.Day13
{
    /**
      * https://adventofcode.com/2020/day/13
      */

    public static class Part01
    {
        private record PuzzleInput(int CurrentTime, int[] Frequency);

        public static void Exec(AOCContext context)
        {
            var rawInput = context.GetInputLines();
            var input = ParseInput(rawInput);
            var (id, waitTime) = CalculateBusInfo(input);

            var result = id * waitTime;
            AOCUtils.PrintResult(result);
        }

        private static PuzzleInput ParseInput(string[] input)
        {
            if (input.Length < 2)
                throw new Exception($"Input must contain at least two lines.");

            var currentTime = int.Parse(input[0], CultureInfo.InvariantCulture);
            var frequency = new List<int>();
            foreach (var item in input[1].Split(',', StringSplitOptions.RemoveEmptyEntries))
            {
                if (item == "x")
                    continue;

                frequency.Add(int.Parse(item, CultureInfo.InvariantCulture));
            }

            return new PuzzleInput(currentTime, frequency.ToArray());
        }

        private static (int id, int waitTime) CalculateBusInfo(PuzzleInput input)
        {
            var currentID = -1;
            var waitTime = int.MaxValue;

            foreach (var item in input.Frequency)
            {
                var itemWait = item - input.CurrentTime % item;
                if (itemWait < waitTime)
                {
                    currentID = item;
                    waitTime = itemWait;
                }
            }

            return (currentID, waitTime);
        }
    }
}