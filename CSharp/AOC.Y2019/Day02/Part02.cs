using AOC.Resources;
using System;

namespace AOC.Y2019.Day02
{
    /**
     * https://adventofcode.com/2019/day/2
     */

    public static class Part02
    {
        private const int ExpectedResult = 19690720;

        public static void Exec()
        {
            string[] input = General.ReadCSVInput(Day.Day02);
            int[] inputIntcode = General.StringToInt(input);
            Span<int> intcode = new int[inputIntcode.Length];

            for (int i = 0; i < 100; i++)
            {
                for (int j = 0; j < 100; j++)
                {
                    inputIntcode.CopyTo(intcode);

                    // Replace initial values.
                    intcode[1] = i;
                    intcode[2] = j;

                    ExecuteIntcode(intcode);

                    if (intcode[0] == ExpectedResult)
                    {
                        General.PrintResult((i * 100) + j);
                        return;
                    }
                }
            }

            Console.WriteLine("No result found");
            Console.ReadKey();
        }

        private static void ExecuteIntcode(Span<int> intcode)
        {
            int index = 0;
            while (intcode[index] != 99)
            {
                // Read input from the two following *addresses*.
                int inputAddress1 = intcode[index + 1];
                int inputAddress2 = intcode[index + 2];

                int result = ExecuteCommand(intcode[index], intcode[inputAddress1], intcode[inputAddress2]);

                // Write result to the third *address*.
                int resultAddress = intcode[index + 3];
                intcode[resultAddress] = result;

                index += 4;
            }
        }

        private static int ExecuteCommand(int opcode, int input1, int input2)
        {
            return opcode switch
            {
                1 => input1 + input2,
                2 => input1 * input2,

                _ => throw new ArgumentException("Invalid intcode"),
            };
        }
    }
}