namespace AOC.Y2020.Day03
{
    /**
      * https://adventofcode.com/2020/day/3
      */

    public static class Part01
    {
        private const char FreeCell = '.';
        private const char TreeCell = '#';

        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var result = GetCollisionCount(input);

            AOCUtils.PrintResult("Count of trees Santa would encounter", result);
        }

        private static int GetCollisionCount(string[] input)
        {
            const int eastInc = 3;
            const int southInc = 1;

            var count = 0;
            var x = 0;

            for (int y = 0; y < input.Length; y += southInc)
            {
                if (input[y][x] == TreeCell)
                {
                    count++;
                }

                x += eastInc;
                if (x >= input[y].Length)
                {
                    x -= input[y].Length;
                }
            }

            return count;
        }
    }
}