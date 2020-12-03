using AOC.Resources;
using AOC.Y2019.Day07.P02;
using System;
using System.Collections.Generic;
using System.Linq;

namespace AOC.Y2019.Day07
{
    /**
     * https://adventofcode.com/2019/day/7
     */

    public static class Part02
    {
        public static void Exec()
        {
            string[] input = General.ReadCSVInput(Day.Day07);
            int[] intcode = General.StringToInt(input);

            try
            {
                int output = CalculateMaxThrusterSignal(intcode);

                WriteLine("The program has halted.", ConsoleColor.Cyan);
                Console.WriteLine("The final output is {0}", output);
            }
            catch (IntcodeEvaluationException e)
            {
                HandleException(e);
            }

            Console.WriteLine("Press a key to close the console.");
            Console.ReadKey();
        }

        private static int CalculateMaxThrusterSignal(int[] intcode)
        {
            int[] phases = new int[] { 5, 6, 7, 8, 9 };
            int maxThrusterSignal = -1;
            int[] maxThrustSignalPhase = new int[phases.Length];

            int amplifierCount = phases.Length;

            int[] phaseInput = new int[amplifierCount];
            IOPort[] ioPorts = new IOPort[amplifierCount];
            IntcodeInterpreter[] amplifiers = new IntcodeInterpreter[amplifierCount];

            int[][] intcodeCopy = new int[amplifierCount][];
            for (int i = 0; i < amplifierCount; i++)
                intcodeCopy[i] = new int[intcode.Length];

            foreach (var phasePermutation in GetPermutations(phases, phases.Length))
            {
                // Copy phases into array for index-access in amplifier loop.
                CopyPhaseInput(phasePermutation, phaseInput);

                // Copy original intcode to array for each amplifier.
                CopyIntcode(intcode, intcodeCopy);

                InitializeIOPorts(ioPorts, phaseInput);
                InitializeAmplifiers(amplifiers, intcodeCopy, ioPorts);

                // First amplifier starts with input 0.
                ioPorts[0].PushInput(0);

                do
                {
                    // Execute each amplifier.
                    for (int i = 0; i < amplifiers.Length; i++)
                    {
                        amplifiers[i].Execute();

                        if (i < amplifiers.Length - 1)
                            ioPorts[i + 1].PushInput(ioPorts[i].Output);
                        else
                            ioPorts[0].PushInput(ioPorts[^1].Output);
                    }
                }
                while (!amplifiers[^1].HasHalted);

                int output = ioPorts[^1].Output;

                // Save the largest output value.
                if (output > maxThrusterSignal)
                {
                    maxThrusterSignal = output;
                    phaseInput.CopyTo(maxThrustSignalPhase, 0);
                }
            }

            Console.Write("The phase was: ");
            for (int i = 0; i < maxThrustSignalPhase.Length; i++)
            {
                Console.Write("{0} ", maxThrustSignalPhase[i]);
            }
            Console.WriteLine();

            return maxThrusterSignal;
        }

        private static void CopyPhaseInput(IEnumerable<int> rawPhases, int[] phaseInput)
        {
            int i = 0;
            foreach (var phase in rawPhases)
            {
                phaseInput[i] = phase;
                i++;
            }
        }

        private static void CopyIntcode(int[] intcode, int[][] destination)
        {
            for (int i = 0; i < destination.Length; i++)
            {
                intcode.CopyTo(destination[i], 0);
            }
        }

        private static void InitializeIOPorts(IOPort[] ioPorts, int[] phaseInput)
        {
            for (int i = 0; i < phaseInput.Length; i++)
            {
                IOPort ioPort = new IOPort();
                ioPort.PushInput(phaseInput[i]);

                ioPorts[i] = ioPort;
            }
        }

        private static void InitializeAmplifiers(IntcodeInterpreter[] amplifiers, int[][] intcode, IOPort[] ioPorts)
        {
            for (int i = 0; i < amplifiers.Length; i++)
            {
                amplifiers[i] = new IntcodeInterpreter(intcode[i], ioPorts[i]);
            }
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
            private readonly Queue<int> _input = new Queue<int>();
            private readonly Queue<IInterpreter> _interpreters = new Queue<IInterpreter>();

            public int Output { get; private set; }

            public IOReadResult Read()
            {
                if (_input.Count > 0)
                    return new IOReadResult(valueAvailable: true, _input.Dequeue());
                else
                    return new IOReadResult(valueAvailable: false, -1);
            }

            public void RegisterInterpreterForInput(IInterpreter interpreter)
            {
                _interpreters.Enqueue(interpreter);
            }

            public void Write(int value)
            {
                Output = value;
            }

            public void PushInput(int input)
            {
                _input.Enqueue(input);

                int interpreters = _interpreters.Count;
                for (int i = 0; i < interpreters; i++)
                {
                    _interpreters.Dequeue().Execute();
                }
            }
        }
    }
}