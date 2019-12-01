using AOC.Resources;
using System.Linq;

namespace day01b
{
    /**
     * https://adventofcode.com/2019/day/1#part2
     */

    internal static class Program
    {
        private static void Main()
        {
            int[] moduleMass = General.ReadIntegerInput(Days.Day01);

            int[] fuelRequirements = CalculateFuelRequirements(moduleMass);
            int result = fuelRequirements.Sum();

            General.PrintResult("The total sum of the fuel requirements is", result);
        }

        private static int[] CalculateFuelRequirements(int[] moduleMass)
        {
            int[] fuelRequirements = new int[moduleMass.Length];
            for (int i = 0; i < moduleMass.Length; i++)
            {
                fuelRequirements[i] = CalculateFuelRequirements(moduleMass[i]);
            }

            return fuelRequirements;
        }

        private static int CalculateFuelRequirements(int mass)
        {
            int fuelRequirement = mass / 3 - 2;
            if (fuelRequirement <= 0)
                return 0;

            fuelRequirement += CalculateFuelRequirements(fuelRequirement);
            return fuelRequirement;
        }
    }
}