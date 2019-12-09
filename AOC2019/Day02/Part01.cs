using AOC.Resources;
using System;

namespace AOC2019.Day02
{
    /**
     * https://adventofcode.com/2019/day/2
     */

    public static class Part01
    {
        public static void Exec()
        {
            string[] input = General.ReadCSVInput(Day.Day02);
            int[] intcode = General.StringToInt(input);

            // Replace initial values.
            intcode[1] = 12;
            intcode[2] = 2;

            ExecuteIntcode(intcode);
            General.PrintResult(intcode[0]);
        }

        private static void ExecuteIntcode(int[] intcode)
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

        private static int ExecuteCommand(int intcode, int input1, int input2)
        {
            return intcode switch
            {
                1 => input1 + input2,
                2 => input1 * input2,

                _ => throw new ArgumentException("Invalid intcode"),
            };
        }
    }
}