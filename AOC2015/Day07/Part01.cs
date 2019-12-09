using AOC.Resources;
using AOC2015.Day07.P01.Binding;
using AOC2015.Day07.P01.Syntax;
using System;
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

            Console.WriteLine("Parsing and binding completed");
            Console.WriteLine();

            foreach (var expression in boundExpressions)
            {
                Console.WriteLine("Signal '{0}' evaluates to {1}", expression.SignalName, expression.Evaluate());
            }
            Console.WriteLine();

            Console.WriteLine("Signal '{0}' evaluates to {1}", ResultSignal, boundExpressions.Single(b => b.SignalName == ResultSignal).Evaluate());

            Console.WriteLine();
            Console.WriteLine("Please come again.");
            Console.ReadKey();
        }
    }
}