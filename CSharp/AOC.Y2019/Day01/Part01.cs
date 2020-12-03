using System.Linq;

namespace AOC.Y2019.Day01
{
    /**
     * https://adventofcode.com/2019/day/1
     */

    public static class Part01
    {
        public static void Exec(AOCContext context)
        {
            int[] moduleMass = context.GetIntegerInput();

            int[] fuelRequirements = CalculateFuelRequirements(moduleMass);
            int result = fuelRequirements.Sum();

            AOCUtils.PrintResult("The sum of the fuel requirements is", result);
        }

        private static int[] CalculateFuelRequirements(int[] moduleMass)
        {
            int[] fuelRequirements = new int[moduleMass.Length];
            for (int i = 0; i < moduleMass.Length; i++)
                fuelRequirements[i] = moduleMass[i] / 3 - 2;

            return fuelRequirements;
        }
    }
}