using AOC.Resources;
using System.Linq;

namespace AOC2019.Day01
{
    /**
     * https://adventofcode.com/2019/day/1#part2
     */

    public static class Part02
    {
        public static void Exec()
        {
            int[] moduleMass = General.ReadIntegerInput(Day.Day01);

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