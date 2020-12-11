using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2020.Day06
{
    /**
      * https://adventofcode.com/2020/day/6
      */

    public static class Part02
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
            var groupedAnswers = GroupAnswers(input);

            var result = new HashSet<char>[groupedAnswers.Count];
            for (int i = 0; i < groupedAnswers.Count; i++)
            {
                result[i] = new();

                var allAnswers = groupedAnswers[i];
                if (allAnswers.Count == 0)
                    continue;

                var firstAnswers = allAnswers[^1];
                foreach (var answer in firstAnswers)
                {
                    if (allAnswers.All(a => a.Contains(answer)))
                        result[i].Add(answer);
                }
            }

            return result;
        }

        private static List<List<HashSet<char>>> GroupAnswers(string[] input)
        {
            if (input.Length == 0)
                return new();

            var groupedAnswers = new List<List<HashSet<char>>> { new() };
            foreach (var line in input)
            {
                if (string.IsNullOrEmpty(line))
                {
                    groupedAnswers.Add(new());
                    continue;
                }

                groupedAnswers[^1].Add(line.ToHashSet());
            }

            return groupedAnswers;
        }
    }
}