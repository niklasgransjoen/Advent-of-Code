namespace AOC.Y2020.Day03
{
    public static class Part02
    {
        private const char FreeCell = '.';
        private const char TreeCell = '#';

        public static void Exec(AOCContext context)
        {
            var input = context.GetInputLines();
            var result = GetCollisionChecksum(input);

            AOCUtils.PrintResult("Collision checksum", result);
        }

        private static long GetCollisionChecksum(string[] input)
        {
            var slopes = new[] { (1, 1), (1, 3), (1, 5), (1, 7), (2, 1) };

            long result = 1;
            foreach (var (southInc, eastInc) in slopes)
            {
                result *= GetCollisionCount(input, southInc, eastInc);
            }

            return result;
        }

        private static int GetCollisionCount(string[] input, int southInc, int eastInc)
        {
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