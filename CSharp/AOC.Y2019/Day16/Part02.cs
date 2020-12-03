using System;
using System.Text;

namespace AOC.Y2019.Day16
{
    /**
     * https://adventofcode.com/2019/day/16
     *
     * This solution works, but will take hours to compute the input.
     */

    public static class Part02
    {
        private const int InputRepeatFactor = 10_000;
        private const int Iterations = 100;
        private const int MessageLength = 8;

        public static void Exec(AOCContext context)
        {
            string rawInput = context.Input;
            byte[] input = ParseInput(rawInput, InputRepeatFactor);

            int messageOffset = LocateMessage(rawInput);
            byte[] output = FFTTransform(input, messageOffset, MessageLength, Iterations);

            string result = MessageToString(output.AsSpan(messageOffset, MessageLength));
            AOCUtils.PrintResult(result);

            while (true)
            {
                Console.ReadLine();
            }
        }

        private static byte[] ParseInput(string input, int repetitions)
        {
            byte[] rawResult = new byte[input.Length];
            for (int i = 0; i < rawResult.Length; i++)
            {
                rawResult[i] = byte.Parse(input.AsSpan(i, 1));
            }

            byte[] result = new byte[input.Length * repetitions];
            for (int i = 0; i < repetitions; i++)
            {
                rawResult.CopyTo(result, i * input.Length);
            }

            return result;
        }

        private static byte[] FFTTransform(byte[] input, int initialPhase, int messageLength, int iterations)
        {
            byte[][] values = ArrayUtility.Create2D(input.Length, iterations + 1, byte.MaxValue);
            input.CopyTo(values[0], 0);

            int counter = 1;
            for (int j = initialPhase + messageLength - 1; j >= initialPhase; j--)
            {
                AOCUtils.WriteLine($"Started resolving message character {counter}/{messageLength}", ConsoleColor.Magenta);
                PhaseShift(values, iterations, j);

                counter++;
            }

            return values[iterations];
        }

        private static void PhaseShift(byte[][] values, int outputLayer, int phase)
        {
            int inputLayer = outputLayer - 1;
            byte[] input = values[inputLayer];
            byte[] output = values[outputLayer];

            int scale = 1 + phase;
            int index = phase; // we can skip straight to phase (scale - 1), because every proceding element is zeroes.

            long result = 0;
            while (index < input.Length)
            {
                int length;

                // Positive.
                length = Math.Min(scale, input.Length - index);
                for (int i = index; i < index + length; i++)
                {
                    if (input[i] == byte.MaxValue)
                        PhaseShift(values, inputLayer, i);

                    result += input[i];
                }

                // Skip zeroes.
                index += scale * 2;
                if (index >= input.Length)
                    break;

                // Negative.
                length = Math.Min(scale, input.Length - index);
                for (int i = index; i < index + length; i++)
                {
                    if (input[i] == byte.MaxValue)
                        PhaseShift(values, inputLayer, i);

                    result -= input[i];
                }

                // Skip zeroes.
                index += scale * 2;
            }

            output[phase] = (byte)Math.Abs(result % 10);
        }

        private static int LocateMessage(string rawInput)
        {
            return int.Parse(rawInput.AsSpan(0, 7));
        }

        private static string MessageToString(Span<byte> message)
        {
            StringBuilder sb = new StringBuilder();
            foreach (var value in message)
                sb.Append(value);

            return sb.ToString();
        }
    }
}