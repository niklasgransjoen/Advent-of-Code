using AOC.Y2019.Day12.P01;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace AOC.Y2019.Day12
{
    /**
     * https://adventofcode.com/2019/day/12
     */

    public static class Part01
    {
        private const long Iterations = 1000;

        public static void Exec(AOCContext context)
        {
            string[] input = context.GetInputLines();
            Moon[] moons = ParseInput(input);
            InitializeMoons(moons);
            SimulateOrbits(moons);

            int result = CalculateEnergy(moons);
            AOCUtils.PrintResult(result);
        }

        private static Moon[] ParseInput(string[] input)
        {
            Moon[] moons = new Moon[input.Length];
            for (int i = 0; i < input.Length; i++)
            {
                string line = input[i];

                Regex regex = new Regex(@"(?<=\=)\-?[0-9]+");
                var matches = regex.Matches(line);

                int x = int.Parse(matches[0].Value);
                int y = int.Parse(matches[1].Value);
                int z = int.Parse(matches[2].Value);

                Vector moonPosition = new Vector(x, y, z);
                moons[i] = new Moon(moonPosition);
            }

            return moons;
        }

        private static void InitializeMoons(Moon[] moons)
        {
            // Create lists of neighbors.
            List<Moon>[] neighborMoons = new List<Moon>[moons.Length];
            for (int i = 0; i < moons.Length; i++) neighborMoons[i] = new List<Moon>();

            // Init lists of neighbors.
            for (int i = 0; i < moons.Length - 1; i++)
            {
                for (int j = i + 1; j < moons.Length; j++)
                {
                    neighborMoons[i].Add(moons[j]);
                    neighborMoons[j].Add(moons[i]);
                }
            }

            // Init moons.
            for (int i = 0; i < moons.Length; i++)
            {
                moons[i].Initialize(neighborMoons[i]);
            }
        }

        private static void SimulateOrbits(Moon[] moons)
        {
            for (long i = 0; i < Iterations; i++)
            {
                // Calculate interactions.
                for (int j = 0; j < moons.Length; j++)
                {
                    moons[j].Interact();
                }

                // Step forward.
                for (int j = 0; j < moons.Length; j++)
                {
                    moons[j].Step();
                }
            }
        }

        private static int CalculateEnergy(Moon[] moons)
        {
            int totalEnergy = 0;

            for (int i = 0; i < moons.Length; i++)
            {
                Moon moon = moons[i];

                int pX = Math.Abs(moon.Position.X);
                int pY = Math.Abs(moon.Position.Y);
                int pZ = Math.Abs(moon.Position.Z);

                int kX = Math.Abs(moon.Velocity.X);
                int kY = Math.Abs(moon.Velocity.Y);
                int kZ = Math.Abs(moon.Velocity.Z);

                totalEnergy += (pX + pY + pZ) * (kX + kY + kZ);
            }

            return totalEnergy;
        }
    }
}