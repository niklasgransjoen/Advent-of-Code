using AOC.Resources;
using System.Linq;

namespace day01
{
    /**
     * https://adventofcode.com/2019/day/1
     */

    internal static class Program
    {
        private static void Main()
        {
            int[] moduleMass = General.ReadIntegerInput(Days.Day01);

            int[] fuelRequirements = CalculateFuelRequirements(moduleMass);
            int result = fuelRequirements.Sum();

            General.PrintResult("The sum of the fuel requirements is", result);
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