namespace AOC.Y2020.Day01
{
    /**
     * https://adventofcode.com/2020/day/1
     */

    public static class Part01
    {
        private const int Checksum = 2020;

        public static void Exec(AOCContext context)
        {
            var input = context.GetIntegerInput();
            var result = GetResult(input);
            if (result is -1)
            {
                AOCUtils.PrintError("No result could be found.");
                return;
            }

            AOCUtils.PrintResult(result);
        }

        private static int GetResult(int[] input)
        {
            for (int i = 0; i < input.Length - 1; i++)
            {
                var item1 = input[i];
                for (int j = i + 1; j < input.Length; j++)
                {
                    var item2 = input[j];

                    if (item1 + item2 == Checksum)
                        return item1 * item2;
                }
            }

            return -1;
        }
    }
}