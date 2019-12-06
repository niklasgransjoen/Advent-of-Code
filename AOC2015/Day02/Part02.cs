using AOC.Resources;
using System.Linq;

namespace AOC2015.Day02
{
    /**
     * https://adventofcode.com/2015/day/2
     */

    public static class Part02
    {
        public static void Exec()
        {
            string[] input = General.ReadInput(Day.Day02);
            Present[] presents = ParseInput(input);

            var wrapperArea = presents.Select(p => p.GetRibbonLength());
            int totalArea = wrapperArea.Sum();

            General.PrintResult(totalArea);
        }

        private static Present[] ParseInput(string[] input)
        {
            Present[] presents = new Present[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string[] dim = input[i].Split('x');

                int length = int.Parse(dim[0]);
                int width = int.Parse(dim[1]);
                int depth = int.Parse(dim[2]);

                presents[i] = new Present(length, width, depth);
            }

            return presents;
        }

        private readonly struct Present
        {
            public Present(int length, int width, int depth)
            {
                Length = length;
                Width = width;
                Depth = depth;
            }

            public int Length { get; }
            public int Width { get; }
            public int Depth { get; }

            public int GetRibbonLength()
            {
                // Find the two smallest values of Length, Width, and Depth.
                int a1 = Length;
                int a2 = Width;
                if (a1 > Depth && a1 > a2)
                    a1 = Depth;
                else if (a2 > Depth)
                    a2 = Depth;

                return 2 * a1 + 2 * a2 + Length * Width * Depth;
            }
        }
    }
}