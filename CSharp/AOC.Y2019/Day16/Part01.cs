using AOC.Resources;
using System;
using System.Linq;

namespace AOC.Y2019.Day16
{
    /**
     * https://adventofcode.com/2019/day/16
     */

    public static class Part01
    {
        private const int Iterations = 100;

        public static void Exec()
        {
            string rawInput = General.ReadSingleLineInput(Day.Day16);
            byte[] input = ParseInput(rawInput);
            FFTTransform(input, Iterations);

            Console.Write("The result is: ");
            for (int i = 0; i < 8; i++)
            {
                Console.Write(input[i]);
            }
            Console.ReadKey();
        }

        private static byte[] ParseInput(string input)
        {
            byte[] result = new byte[input.Length];
            for (int i = 0; i < result.Length; i++)
            {
                result[i] = byte.Parse(input.AsSpan(i, 1));
            }

            return result;
        }

        private static void FFTTransform(byte[] input, int iterations)
        {
            for (int i = 0; i < iterations; i++)
            {
                for (int j = 0; j < input.Length; j++)
                {
                    PhaseShift(input, j);
                }
            }
        }

        private static void PhaseShift(byte[] values, int phase)
        {
            int scale = 1 + phase;
            int index = phase; // we can skip straight to phase (scale - 1), because every proceding element is zeroes.
            Span<byte> span = values.AsSpan();

            int result = 0;
            while (index < values.Length)
            {
                int length;

                // Positive.
                length = Math.Min(scale, values.Length - index);
                foreach (var value in span.Slice(index, length))
                    result += value;

                // Skip zeroes.
                index += scale * 2;
                if (index >= values.Length)
                    break;

                // Negative.
                length = Math.Min(scale, values.Length - index);
                foreach (var value in span.Slice(index, length))
                    result -= value;

                // Skip zeroes.
                index += scale * 2;
            }

            values[phase] = (byte)Math.Abs(result % 10);
        }
    }
}