using AOC.Y2015.Day07.P02;
using AOC.Y2015.Day07.P02.Binding;
using AOC.Y2015.Day07.P02.Syntax;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2015.Day07
{
    /**
     * https://adventofcode.com/2015/day/7
     *
     * "Over-engineering is my middle name" - Me
     */

    public static class Part02
    {
        private const string FirstSignal = "a";
        private const string SecondSignal = "b";

        public static void Exec(AOCContext context)
        {
            string input = context.Input;
            var expressions = Parser.Parse(input);
            var boundExpressions = Binder.Bind(expressions);

            // Find value of first signal.
            BoundExpression firstSignal = LocateSignal(boundExpressions, FirstSignal);
            Evaluator evaluator = new Evaluator();
            ushort firstSignalValue = evaluator.Evaluate(firstSignal);

            // Force first signal into second signal and recalculate with new evaluator.
            evaluator = new Evaluator();
            BoundExpression secondSignal = LocateSignal(boundExpressions, SecondSignal);
            evaluator.ForceSignalValue(secondSignal, firstSignalValue);

            ushort result = evaluator.Evaluate(firstSignal);
            AOCUtils.PrintResult(result);
        }

        private static BoundExpression LocateSignal(IEnumerable<BoundExpression> expressions, string signalName)
        {
            return expressions.Single(b => b.SignalName == signalName);
        }
    }
}