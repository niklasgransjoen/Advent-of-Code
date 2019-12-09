using AOC.Resources;
using AOC2015.Day07.P01;
using AOC2015.Day07.P01.Binding;
using AOC2015.Day07.P01.Syntax;
using System.Linq;

namespace AOC2015.Day07
{
    /**
     * https://adventofcode.com/2015/day/7
     *
     * "Over-engineering is my middle name" - Me
     */

    public static class Part01
    {
        private const string ResultSignal = "a";

        public static void Exec()
        {
            string input = General.ReadSingleLineInput(Day.Day07);
            var expressions = Parser.Parse(input);
            var boundExpressions = Binder.Bind(expressions);

            BoundExpression? resultSignal = boundExpressions.SingleOrDefault(b => b.SignalName == ResultSignal);
            if (resultSignal is null)
            {
                General.PrintError($"Signal with name '{ResultSignal}' does not exist.");
                return;
            }

            Evaluator evaluator = new Evaluator();
            ushort result = evaluator.Evaluate(resultSignal);

            General.PrintResult(result);
        }
    }
}