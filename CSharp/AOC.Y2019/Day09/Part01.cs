using AOC.Resources;
using AOC.Y2019.Day09.P01;
using System;

namespace AOC.Y2019.Day09
{
    /**
     * https://adventofcode.com/2019/day/9
     */

    public static class Part01
    {
        public static void Exec()
        {
            string[] input = General.ReadCSVInput(Day.Day09);
            long[] intcode = General.StringToLong(input);

            IntcodeInterpreter interpreter = new IntcodeInterpreter(intcode, new IOPort());

            try
            {
                interpreter.Execute();

                if (interpreter.HasHalted)
                    WriteLine("The program has halted.", ConsoleColor.Cyan);
                else
                    WriteLine("Program execution ended without halting.", ConsoleColor.DarkRed);
            }
            catch (IntcodeEvaluationException e)
            {
                HandleException(e);
            }

            Console.ReadKey();
        }

        #region Intcode Utilities

        private static void HandleException(IntcodeEvaluationException ex)
        {
            Console.WriteLine();
            WriteLine("The intcode interpreter encountered an error.", ConsoleColor.DarkMagenta);

            WriteLine(ex.Message, ConsoleColor.DarkRed);
            Console.WriteLine();
        }

        #endregion Intcode Utilities

        #region Helpers

        private static void WriteLine(string value, ConsoleColor foreground)
        {
            Console.ForegroundColor = foreground;
            Console.WriteLine(value);
        }

        #endregion Helpers

        private sealed class IOPort : IIOPort
        {
            public IOReadResult Read()
            {
                const int input = 1;

                return new IOReadResult(input);
            }

            public void RegisterInterpreterForInput(IInterpreter interpreter)
            {
                throw new IntcodeEvaluationException("IO Port does not support registrations for inputs.");
            }

            public void Write(long value)
            {
                Console.WriteLine(value);
            }
        }
    }
}