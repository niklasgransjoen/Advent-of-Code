using AOC.Y2019.Day07.P01;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2019.Day07
{
    /**
     * https://adventofcode.com/2019/day/7
     */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            string[] input = context.GetCSVInput();
            int[] intcode = AOCUtils.StringToInt(input);

            try
            {
                var interpreter = InitInterpreter();
                int output = CalculateMaxThrusterSignal(interpreter, intcode);
                EvaluateOutput(output);
            }
            catch (IntcodeEvaluationException e)
            {
                HandleException(e);
            }

            Console.WriteLine("Press a key to close the console.");
            Console.ReadKey();
        }

        private static int CalculateMaxThrusterSignal(IntcodeInterpreter interpreter, int[] intcode)
        {
            const int AMPLIFIERCOUNT = 5;

            const int INPUTCOUNT = 2;
            const int PHASEINPUT = 0;
            const int AMPINPUT = 1;

            int[] phases = new int[] { 0, 1, 2, 3, 4 };
            int maxThrusterSignal = -1;

            int[] phaseInput = new int[phases.Length];
            int[] intcodeInputs = new int[INPUTCOUNT];
            int[] intcodeCopy = new int[intcode.Length];

            foreach (var phasePermutation in GetPermutations(phases, phases.Length))
            {
                // Copy phases into array for index-access in amplifier loop.
                int phaseIndex = 0;
                foreach (var phase in phasePermutation)
                {
                    phaseInput[phaseIndex] = phase;
                    phaseIndex++;
                }

                int ampInput = 0;
                int output = -1;

                // Execute each amplifier.
                for (int i = 0; i < AMPLIFIERCOUNT; i++)
                {
                    // Reset intcode.
                    intcode.CopyTo(intcodeCopy, 0);

                    // Init input
                    intcodeInputs[PHASEINPUT] = phaseInput[i];
                    intcodeInputs[AMPINPUT] = ampInput;

                    output = interpreter.Execute(intcodeCopy, intcodeInputs);
                    ampInput = output;
                }

                // Save the largest output value.
                if (output > maxThrusterSignal)
                    maxThrusterSignal = output;
            }

            return maxThrusterSignal;
        }

        private static IEnumerable<IEnumerable<int>> GetPermutations(IEnumerable<int> phases, int length)
        {
            if (length == 1)
                return phases.Select(p => Yield(p));

            return GetPermutations(phases, length - 1)
                    .SelectMany(t => phases.Where(p => !t.Contains(p)),
                        (t, p) => t.Concat(Yield(p)));

            static IEnumerable<int> Yield(int phase)
            {
                yield return phase;
            }
        }

        #region Intcode Utilities

        private static IntcodeInterpreter InitInterpreter()
        {
            WriteLine("Intcode execution initiated", ConsoleColor.Cyan);
            Console.WriteLine();

            return new IntcodeInterpreter();
        }

        private static void EvaluateOutput(int output)
        {
            Console.WriteLine();
            WriteLine("The program has halted.", ConsoleColor.Cyan);
            Console.WriteLine("The final output is {0}", output);
        }

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
    }
}