using AOC.Resources;

namespace day03a
{
    /*
     * The whole piece of fabric they're working on is a very large square - at least 1000 inches on each side.
     * Each Elf has made a claim about which area of fabric would be ideal for Santa's suit. All claims have an
     * ID and consist of a single rectangle with edges parallel to the edges of the fabric.
     *
     * A claim like "#123 @ 3,2: 5x4" means that claim ID 123 specifies a rectangle 3 inches from the left edge,
     * 2 inches from the top edge, 5 inches wide, and 4 inches tall.
     *
     * How many square inches of fabric are within two or more claims?
     *
     * https://adventofcode.com/2018/day/3
     */

    internal static class Program
    {
        private static void Main()
        {
            string[] input = General.ReadInput(Days.Day03);
            int result = FindOverlappingClaims(input);

            General.PrintResult("Square inches of fabric claimed more than once", result);
        }

        /// <summary>
        /// Returns an array containing all overlapping fields.
        /// </summary>
        private static int FindOverlappingClaims(string[] input)
        {
            bool[,] claims = new bool[1000, 1000];
            int overlappingClaims = 0;

            foreach (string line in input)
            {
                // Extract the useful information
                string[] rawInput = line.Split(' ');

                string[] rawCoordinates = rawInput[2].Split(new char[] { ',', ':' });
                int x = int.Parse(rawCoordinates[0]);
                int y = int.Parse(rawCoordinates[1]);

                string[] rawDimensions = rawInput[3].Split('x');
                int width = int.Parse(rawDimensions[0]);
                int height = int.Parse(rawDimensions[1]);

                for (int i = x; i < x + width; i++)
                    for (int j = y; j < y + height; j++)
                    {
                        if (!claims[i, j])
                            claims[i, j] = true;
                        else
                            overlappingClaims++;
                    }
            }

            return overlappingClaims;
        }
    }
}