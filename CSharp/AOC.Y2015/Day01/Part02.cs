using System;

namespace AOC.Y2015.Day01
{
    /**
     * https://adventofcode.com/2015/day/1
     */

    public static class Part02
    {
        public static void Exec(AOCContext context)
        {
            string input = context.Input;

            int currentFloor = 0;
            for (int i = 0; i < input.Length; i++)
            {
                if (input[i] == '(')
                    currentFloor++;
                else
                    currentFloor--;

                if (currentFloor == -1)
                {
                    int result = i + 1;
                    AOCUtils.PrintResult(result);
                    return;
                }
            }

            Console.WriteLine("Santa never reaches the basement.");
            Console.ReadKey();
        }
    }
}