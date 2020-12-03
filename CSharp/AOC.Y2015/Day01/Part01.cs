using AOC.Resources;
using System.Linq;

namespace AOC.Y2015.Day01
{
    /**
     * https://adventofcode.com/2015/day/1
     */

    public static class Part01
    {
        public static void Exec()
        {
            string input = General.ReadSingleLineInput(Day.Day01);
            int positive = input.Count(c => c == '(');
            int negative = input.Length - positive;

            int result = positive - negative;
            General.PrintResult(result);
        }
    }
}