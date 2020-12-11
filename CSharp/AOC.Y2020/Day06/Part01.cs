using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2020.Day06
{
    /**
      * https://adventofcode.com/2020/day/6
      */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var answersPerGroup = GetAnswersPerGroup(input);

            var result = answersPerGroup.Sum(answers => answers.Count);
            AOCUtils.PrintResult(result);
        }

        private static HashSet<char>[] GetAnswersPerGroup(string[] input)
        {
            var result = new List<HashSet<char>> { new() };
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    result.Add(new());
                    continue;
                }

                foreach (var answer in line)
                {
                    result[^1].Add(answer);
                }
            }

            return result.ToArray();
        }
    }
}